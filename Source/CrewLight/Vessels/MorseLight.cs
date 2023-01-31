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
using UnityEngine;
using KSPe.Annotations;
using LASL.KSP.Misc;
using SwitchLights = LASL.KSP.Support.SwitchLights;

namespace CrewLight
{
	public class MorseLight : MonoBehaviour, GameSettingsLive.IUpdateable, Morse.IPlay
	{
		private Vessel vessel;

		private WaitForSeconds timer;
		private CL_GeneralSettings settings;
		private SwitchLights.Regime regime;
		private Morse morse;
		private string morseCodeStr = "";

		[UsedImplicitly]
		private void Start ()
		{
			vessel = this.GetComponent<Vessel> ();
			Log.dbg("MorseLight Start for {0} {1}", this.vessel, this.enabled);

			timer = new WaitForSeconds (.5f);

			// Check for the right type
			if (vessel.vesselType == VesselType.Debris || vessel.vesselType == VesselType.EVA
				|| vessel.vesselType == VesselType.Flag || vessel.vesselType == VesselType.SpaceObject)
			{
				Log.dbg("MorseLight Destroyed for {0} due being unsupported type.", this.vessel);
				Destroy (this);
			}

			this.settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();

			// Check for controllable vessel
			if (settings.onlyForControllable) {
				if (!vessel.IsControllable) {
					Log.dbg("MorseLight Destroyed for {0} because it's not controllable.", this.vessel);
					Destroy (this);
				}
			}

			// Destroy if vessel are too close
			if (GetDistance() < settings.distanceMin) {
				Log.dbg("MorseLight Destroyed for {0} because it's too close.", this.vessel);
				Destroy (this);
			}

			this.morse = new Morse(this.settings);
			this.CreateRegime();
			StartCoroutine ("StartMorseLight");
		}

		void GameSettingsLive.IUpdateable.Update() => this.CreateRegime();
		private void CreateRegime() => this.regime =
			new SwitchLights.Regime(this.GetType().Name, true, true, true, true);


		[UsedImplicitly]
		private void OnDestroy ()
		{
			StopAllCoroutines ();
			Registry.SwitchLights.Instance[this.vessel].PopState();
		}

		private double GetDistance() => Vector3d.Distance(FlightGlobals.ActiveVessel.orbit.pos, vessel.orbit.pos);

		private IEnumerator StartMorseLight ()
		{
			Log.dbg("MorseLight started for {0}", this.vessel);
			double vesselDistance = GetDistance ();
			while (vesselDistance > settings.distanceMin)
			{
				while (vesselDistance > settings.distance) {
					if (vesselDistance > settings.distanceMax) {
						Log.dbg("MorseLight Destroyed for {0} because it's too far!", this.vessel);
						Destroy (this);
					}
					yield return timer;
					vesselDistance = this.GetDistance ();
				}

				Registry.SwitchLights.Instance[this.vessel].PushState();
				Registry.SwitchLights.Instance[this.vessel].TurnOff(this.regime);
				yield return new WaitForSeconds(settings.ditDuration);

				// Morse message
				if (!this.settings.morseCodeStr.Equals(this.morseCodeStr))
				{ 
					this.morse.EncodeFromAny(this.settings.morseCodeStr);
					this.morseCodeStr = this.settings.morseCodeStr;
				}
				Log.dbg("Playing morse code {0}", this.morseCodeStr);
				yield return this.morse.PlayCoroutine(this);
				vesselDistance = this.settings.playOnce ? 0 : this.GetDistance();
			}

			Destroy(this);
		}

		void Morse.IPlay.RisingEdge()
		{
			Registry.SwitchLights.Instance[this.vessel].TurnOn(this.regime);
		}

		void Morse.IPlay.FallingEdge()
		{
			Registry.SwitchLights.Instance[this.vessel].TurnOff(this.regime);
		}
	}
}

