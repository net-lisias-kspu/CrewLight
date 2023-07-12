/*
	This file is part of Crew Light /L Unleashed
		© 2021-2023 LisiasT : http://lisias.net <support@lisias.net>
		© 2016-2019 Lion-O

	CrewLight is double licensed, as follows:
		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
		* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	Crew Light /L Unleashed is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with Crew Light /L Unleashed.
	If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with Crew Light /L Unleashed.
	If not, see <https://www.gnu.org/licenses/>.

*/
using System.Collections.Generic;

using UnityEngine;
using KSP.Localization;

using GUILayout = KSPe.UI.GUILayout;
using Asset = KSPe.IO.Asset<CrewLight.Startup>;
using KSPe.Annotations;

namespace CrewLight
{
	[KSPAddon(KSPAddon.Startup.EveryScene, true)]
	public class GameSettingsLive : MonoBehaviour
	{
		internal interface IUpdateable
		{
			void Update();
		}

		internal static readonly List<IUpdateable> updateables = new List<IUpdateable>();
		public static bool inSunlight = true;
		public static int layerMask = (1 << 10 | 1 << 15); // Scaled & Local Scenery layer

		private CL_GeneralSettings settings;

		// Backup :
		private string bckCode;
		private float bckDih, bckDah, bckSymSpace, bckLetterSpace, bckWordSpace;
		private bool bckManual;
		private bool restoreBck = true;

		// window pos
		Vector2d windowPos;

		private Texture morseAlph;

		[UsedImplicitly]
		private void Start ()
		{
			GameEvents.OnGameSettingsApplied.Add (SettingsApplied);
			GameEvents.onGameStateLoad.Add (GameLoad);
			GameEvents.onGameUnpause.Add (OutOfPause);

			windowPos = new Vector2d (Screen.width / 2 + 120, Screen.height / 2 - 150);
			morseSettingsRect = new Rect ((float)windowPos.x, (float)windowPos.y, 1, 1);
			morseAlphabetRect = new Rect ((float)windowPos.x - 680, (float)windowPos.y, 450, 450);

			DoStart ();

			DontDestroyOnLoad (this);
		}

		private void DoStart ()
		{
			if (null != HighLogic.fetch.currentGame) return;

			morseAlph = Asset.Texture2D.LoadFromFile("International_Morse_Code");

			settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
		}

		[UsedImplicitly]
		private void OnDestroy ()
		{
			GameEvents.OnGameSettingsApplied.Remove (SettingsApplied);
			GameEvents.onGameStateLoad.Remove (GameLoad);
			GameEvents.onGameUnpause.Remove (OutOfPause);
		}

		private void GameLoad (ConfigNode node)
		{
			DoStart ();
		}

		private void OutOfPause ()
		{
			CloseSettings ();
			restoreBck = true;
		}

		private void SettingsApplied ()
		{
			if (null == this.settings) return;
			if (null == HighLogic.fetch.currentGame) return;
			if (null == HighLogic.CurrentGame) return;	// Being extra careful

			// execute once when leaving the stock setting screen

			if (settings.distanceMax > 2600)
				settings.distanceMax = 2600;

			if (settings.distanceMax <= settings.distance)
				settings.distance = settings.distanceMax / 5;

			if (settings.distance <= settings.distanceMin)
				settings.distanceMin = settings.distance / 5;

			if (settings.morseConf)
			{
				// reset the more morse conf toggle to false asap
				settings.morseConf = false;
				settings.Save (HighLogic.CurrentGame.config);

				showSettingsWindow = true;
			}

			// backup the original settings
			ParseToBackup ();
			this.CallUpdateables();
		}

		private void CallUpdateables()
		{
			foreach(IUpdateable i in updateables) i.Update();
		}

		private void ParseToBackup ()
		{
			bckCode = settings.morseCodeStr;
			bckDih = settings.ditDuration;
			bckDah = settings.dahDuration;
			bckSymSpace = settings.symbolSpaceDuration;
			bckLetterSpace = settings.letterSpaceDuration;
			bckWordSpace = settings.wordSpaceDuration;
			bckManual = settings.manualTiming;
		}

		private void RestoreBackup ()
		{
			settings.morseCodeStr = bckCode;
			settings.ditDuration = bckDih;
			settings.dahDuration = bckDah;
			settings.symbolSpaceDuration = bckSymSpace;
			settings.letterSpaceDuration = bckLetterSpace;
			settings.wordSpaceDuration = bckWordSpace;
			settings.manualTiming = bckManual;
		}

		private void ApplySettings ()
		{
			settings.Save (HighLogic.CurrentGame.config);
			showSettingsWindow = false;
			showAlphabetWindow = false;

			restoreBck = false;
		}

		private void CloseSettings ()
		{
			if (restoreBck)
			{
				RestoreBackup ();
			}

			showSettingsWindow = false;
			showAlphabetWindow = false;
		}

		#region MorseGUI

		private bool showSettingsWindow = false;
		private bool showAlphabetWindow = false;

		private Rect morseSettingsRect;
		private Rect morseAlphabetRect;

		[UsedImplicitly]
		private void OnGUI ()
		{
			if (showSettingsWindow)
			{
				GUILayout.BeginArea (morseSettingsRect);
				morseSettingsRect = GUILayout.Window (991237, morseSettingsRect, MorseSettingsWindow, Localizer.Format ("#autoLOC_CL_0077"));
				GUILayout.EndArea ();
			}

			if (showAlphabetWindow)
			{
				GUILayout.BeginArea (morseAlphabetRect);
				morseAlphabetRect = GUILayout.Window (596064, morseAlphabetRect, MorseAlphabetWindow, Localizer.Format ("#autoLOC_CL_0078"));
				GUILayout.EndArea ();
			}
		}

		public void MorseSettingsWindow (int id)
		{
			GUILayout.BeginVertical ();

			settings.morseCodeStr = GUILayout.TextField (settings.morseCodeStr);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (/*Dit*/Localizer.Format ("#autoLOC_CL_0029") + " (.)")) {
				settings.morseCodeStr += ".";
			}
			if (GUILayout.Button (/*"Dah*/Localizer.Format ("#autoLOC_CL_0032") +  " (_)")) {
				settings.morseCodeStr += "_";
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (/*"Letter Space */Localizer.Format ("#autoLOC_CL_0036") + " ( )")) {
				settings.morseCodeStr += " ";
			}
			if (GUILayout.Button (/*"Word Space*/Localizer.Format ("#autoLOC_CL_0038") + " (|)")) {
				settings.morseCodeStr += "|";
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label (/*"Dih duration :"*/Localizer.Format ("#autoLOC_CL_0030"));
			if (GUILayout.Button ("--")) {
				settings.ditDuration -= .1f;
				UpdateTiming ();
			}
			if (GUILayout.Button ("-")) {
				settings.ditDuration -= .01f;
				UpdateTiming ();
			}
			GUILayout.Label (settings.ditDuration.ToString ());
			if (GUILayout.Button ("+")) {
				settings.ditDuration += .01f;
				UpdateTiming ();
			}

			if (GUILayout.Button ("++")) {
				settings.ditDuration += .1f;
				UpdateTiming ();
			}
			GUILayout.EndHorizontal ();

			settings.manualTiming = GUILayout.Toggle (settings.manualTiming, /*"Manual Timing"*/Localizer.Format ("#autoLOC_CL_0031"));

			GUILayout.BeginHorizontal ();
			GUILayout.Label (/*"Dah duration :"*/Localizer.Format ("#autoLOC_CL_0033"));
			if (GUILayout.Button ("--")) {
				if (settings.manualTiming) {
					settings.dahDuration -= .1f;
				}
			}
			if (GUILayout.Button ("-")) {
				if (settings.manualTiming) {
					settings.dahDuration -= .01f;
				}
			}
			GUILayout.Label (settings.dahDuration.ToString ());
			if (GUILayout.Button ("+")) {
				if (settings.manualTiming) {
					settings.dahDuration += .01f;
				}
			}
			if (GUILayout.Button ("++")) {
				if (settings.manualTiming) {
					settings.dahDuration += .1f;
				}
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label (/*"Symbol Space :"*/Localizer.Format ("#autoLOC_CL_0034"));
			if (GUILayout.Button ("--")) {
				if (settings.manualTiming) {
					settings.symbolSpaceDuration -= .1f;
				}
			}
			if (GUILayout.Button ("-")) {
				if (settings.manualTiming) {
					settings.symbolSpaceDuration -= .01f;
				}
			}
			GUILayout.Label (settings.symbolSpaceDuration.ToString ());
			if (GUILayout.Button ("+")) {
				if (settings.manualTiming) {
					settings.symbolSpaceDuration += .01f;
				}
			}
			if (GUILayout.Button ("++")) {
				if (settings.manualTiming) {
					settings.symbolSpaceDuration += .1f;
				}
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label (/*"Letter Space :"*/Localizer.Format ("#autoLOC_CL_0036"));
			if (GUILayout.Button ("--")) {
				if (settings.manualTiming) {
					settings.letterSpaceDuration -= .1f;
				}
			}
			if (GUILayout.Button ("-")) {
				if (settings.manualTiming) {
					settings.letterSpaceDuration -= .01f;
				}
			}
			GUILayout.Label (settings.letterSpaceDuration.ToString ());
			if (GUILayout.Button ("+")) {
				if (settings.manualTiming) {
					settings.letterSpaceDuration += .01f;
				}
			}
			if (GUILayout.Button ("++")) {
				if (settings.manualTiming) {
					settings.letterSpaceDuration += .1f;
				}
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label (/*"Word Space :"*/Localizer.Format ("#autoLOC_CL_0038"));
			if (GUILayout.Button ("--")) {
				if (settings.manualTiming) {
					settings.wordSpaceDuration -= .1f;
				}
			}
			if (GUILayout.Button ("-")) {
				if (settings.manualTiming) {
					settings.wordSpaceDuration -= .01f;
				}
			}
			GUILayout.Label (settings.wordSpaceDuration.ToString ());
			if (GUILayout.Button ("+")) {
				if (settings.manualTiming) {
					settings.wordSpaceDuration += .01f;
				}
			}
			if (GUILayout.Button ("++")) {
				if (settings.manualTiming) {
					settings.wordSpaceDuration += .1f;
				}
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (/*"Cancel"*/Localizer.Format ("#autoLOC_CL_0079"))) {
				CloseSettings ();
			}
			if (GUILayout.Button (/*"Morse Alphabet"*/Localizer.Format ("#autoLOC_CL_0078"))) {
				showAlphabetWindow = !showAlphabetWindow;
			}
			if (GUILayout.Button (/*"Apply"*/Localizer.Format ("#autoLOC_CL_0080"))) {
				ApplySettings ();
			}
			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();

			GUI.DragWindow ();
		}

		public void MorseAlphabetWindow (int id)
		{
			GUILayout.Label (morseAlph);

			GUI.DragWindow ();
		}

		private void UpdateTiming ()
		{
			if (!settings.manualTiming)
			{
				settings.dahDuration = settings.ditDuration * 3;
				settings.symbolSpaceDuration = settings.ditDuration;
				settings.letterSpaceDuration = settings.dahDuration;
				settings.wordSpaceDuration = settings.dahDuration * 3;
			}
		}
		#endregion
	}
}

