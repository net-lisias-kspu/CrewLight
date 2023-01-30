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
using System.Collections;
using System.Collections.Generic;
using KSPe.Annotations;
using UnityEngine;

namespace CrewLight
{
	public enum MorseCode { dih, dah, letterspc, wordspc, symspc };

	public class MorseLight : MonoBehaviour
	{
		private Vessel vessel;
		private List<PartModule> modulesLight;
		private List<bool?> stateLight;
		private double offLimit = 2600d;

		private WaitForSeconds timer;
		private bool isRunning = false;

		private CL_GeneralSettings settings;

		[UsedImplicitly]
		private void Start ()
		{
			settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();

			timer = new WaitForSeconds (.5f);

			vessel = this.GetComponent<Vessel> ();

			// Check for the right type
			if (vessel.vesselType == VesselType.Debris || vessel.vesselType == VesselType.EVA
				|| vessel.vesselType == VesselType.Flag || vessel.vesselType == VesselType.SpaceObject) {

				Destroy (this);
			}

			// Check for controllable vessel
			if (settings.onlyForControllable) {
				if (!vessel.IsControllable) {
					Destroy (this);
				}
			}

			// Destroy if vessel are too close
			if (GetDistance () < settings.distance) {
				Destroy (this);
			}

			StartCoroutine ("StartMorseLight");
		}

		[UsedImplicitly]
		private void OnDestroy ()
		{
			StopAllCoroutines ();
			if (isRunning) { LightPreviousState (); }
		}

		private double GetDistance ()
		{
			return Vector3d.Distance (FlightGlobals.ActiveVessel.orbit.pos, vessel.orbit.pos);
		}

		private IEnumerator StartMorseLight ()
		{
			isRunning = true;

			yield return StartCoroutine ("FindLightPart");
			double vesselDistance = GetDistance ();
			while (vesselDistance > settings.distance) {
				if (vesselDistance > offLimit) {
					Destroy (this);
				}
				yield return timer;
				vesselDistance = GetDistance ();
			}

			SwitchLight.Instance.Off(modulesLight);
			yield return new WaitForSeconds (settings.ditDuration);
			// Morse message
			foreach (MorseCode c in GameSettingsLive.morseCode) {
				switch (c) {
				case MorseCode.dih:
					SwitchLight.Instance.On(modulesLight);
					yield return new WaitForSeconds (settings.ditDuration);
					break;
				case MorseCode.dah:
					SwitchLight.Instance.On(modulesLight);
					yield return new WaitForSeconds (settings.dahDuration);
					break;
				case MorseCode.letterspc:
					SwitchLight.Instance.Off(modulesLight);
					yield return new WaitForSeconds (settings.letterSpaceDuration);
					break;
				case MorseCode.wordspc:
					SwitchLight.Instance.Off(modulesLight);
					yield return new WaitForSeconds (settings.wordSpaceDuration);
					break;
				case MorseCode.symspc:
					SwitchLight.Instance.Off(modulesLight);
					yield return new WaitForSeconds (settings.symbolSpaceDuration);
					break;
				}
			}
			LightPreviousState ();
			isRunning = false;
			Destroy (this);
		}

		private void LightPreviousState ()
		{
			// Settings lights to theirs previous state
			if (stateLight != null && modulesLight != null) {
				int i = 0;
				foreach (bool? isOn in stateLight) {
					if (isOn == null) {
						if (modulesLight [i].part.CrewCapacity > 0) {
							if (modulesLight [i].part.protoModuleCrew.Count > 0) {
								SwitchLight.Instance.On(modulesLight [i].part);
							} else {
								SwitchLight.Instance.Off(modulesLight [i].part);
							}
						}
					} else if (isOn == true) {
						SwitchLight.Instance.On(modulesLight [i].part);
					} else {
						SwitchLight.Instance.Off(modulesLight [i].part);
					}
					i++;
				}
			}
		}

		private IEnumerator FindLightPart ()
		{
			modulesLight = new List<PartModule> ();

			stateLight = new List<bool?> ();

			int iSearch = -1;

			yield return new WaitForSeconds (.1f);

			foreach (Part part in vessel.Parts) {
				iSearch++;
				if (iSearch >= GameSettingsLive.maxSearch) {
					yield return new WaitForSeconds (.1f);
					iSearch = 0;
				}

				// Check for retractable landing gear/whell
				if (part.Modules.Contains<ModuleStatusLight> ()) {
					break;
				}

				foreach (PartModule partM in part.Modules) if (Support.Facade.IsSupported(partM))
				{ 
					modulesLight.Add(partM);
					stateLight.Add(Support.Facade.Instance(partM).IsOn(partM));
				}
			}
		}
	}
}

