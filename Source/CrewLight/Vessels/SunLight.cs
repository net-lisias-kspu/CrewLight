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
using UnityEngine;
using KSPe.Annotations;
using SwitchLights = LASL.KSP.Support.SwitchLights;

namespace CrewLight
{
	public class SunLight : MonoBehaviour, GameSettingsLive.IUpdateable
	{
		private Vessel vessel;
		private bool inDark;

		private CL_SunLightSettings settings;
		private CL_GeneralSettings generalSettings;

		private SwitchLights.Regime regime;

		[UsedImplicitly]
		private void Start ()
		{
			settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_SunLightSettings> ();
			generalSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();

			vessel = this.GetComponent<Vessel> ();

			// Checking for the type cannot be done earlier unfortunalely, it won't be correctly assigned
			if (vessel.vesselType == VesselType.Debris || vessel.vesselType == VesselType.EVA 
				|| vessel.vesselType == VesselType.Flag || vessel.vesselType == VesselType.SpaceObject) {

				Destroy (this);
			}

			this.CreateRegime();
			GameSettingsLive.updateables.Add(this);
			StartCoroutine ("StartSunLight");
		}

		void GameSettingsLive.IUpdateable.Update() => this.CreateRegime();
		private void CreateRegime() => this.regime =
			new SwitchLights.Regime(this.GetType().Name, !generalSettings.useVesselLightsOnEVA, false, !generalSettings.useTransferCrew, false);

		[UsedImplicitly]
		private void OnDestroy ()
		{
			StopAllCoroutines ();
			GameSettingsLive.updateables.Remove(this);
		}

		private bool IsSunShine ()
		{
			Vector3d vesselPos = vessel.GetWorldPos3D ();
			Vector3d sunPos = FlightGlobals.GetBodyByName ("Sun").position;
			RaycastHit hit;

			if (Physics.Raycast(vesselPos, sunPos, out hit, Mathf.Infinity, GameSettingsLive.layerMask)) {
				if (hit.transform.name == "Sun") {
					return true;
				}
			}
			return false;
		}

		private bool IsInDepth ()
		{
			if (vessel.LandedOrSplashed && FlightGlobals.currentMainBody.ocean) {
				if (vessel.altitude < -settings.depthThreshold) {
					return true;
				}
			}
			return false;
		}
			
		private void SetLights ()
		{
			// Depth Lights :
			if (settings.useDepthLight) {
				if (IsInDepth ()) {
					if (!inDark) {
						if (settings.useSunLight) {
							StartCoroutine ("StageLight");
						} else {
							Registry.SwitchLights.Instance[this.vessel].TurnOn();
						}
						inDark = true;
					}
					return;
				}
			}

			// Sun Lights :
			if (IsSunShine()) {
				if (inDark) {
					StopCoroutine ("StageLight");
					Registry.SwitchLights.Instance[this.vessel].TurnOff();
					inDark = false;
				}
			} else {
				if (!inDark) {
					if (settings.useStaggeredLight) {
						StartCoroutine ("StageLight");
					} else {
						Registry.SwitchLights.Instance[this.vessel].TurnOn();
					}
					inDark = true;
				}
			}
		}

		[UsedImplicitly]
		private IEnumerator StartSunLight ()
		{
			inDark = IsSunShine (); 

			while (true) {
				SetLights ();
				if (TimeWarp.CurrentRate < 5f) {
					yield return new WaitForSeconds (settings.delayLowTimeWarp / TimeWarp.CurrentRate);
				} else {
					yield return new WaitForSeconds (settings.delayHighTimeWarp);
				}
			}
		}

		[UsedImplicitly]
		private IEnumerator StageLight ()
		{
			foreach (List<Part> stageList in SliceLightList()) {
				Registry.SwitchLights.Instance[this.vessel].TurnOn(stageList);
				if (settings.useRandomDelay) {
					yield return new WaitForSeconds (UnityEngine.Random.Range (.4f, 2f));
				} else {
					yield return new WaitForSeconds (settings.delayStage);
				}
			}
		}

		private List<List<Part>> SliceLightList ()
		{
			List<List<Part>> slicedList = new List<List<Part>>();
			List<Part> workingList = new List<Part>(Registry.SwitchLights.Instance[this.vessel].PartsWithLight());

			while (0 != workingList.Count)
			{
				List<Part> stageList = new List<Part> ();
				int rndLightInStage = UnityEngine.Random.Range (settings.minLightPerStage, settings.maxLightPerStage);
				if (rndLightInStage > workingList.Count) {
					rndLightInStage = workingList.Count;
				}
				for (int i = 0 ; i < rndLightInStage ; i++) {
					int randIndex = UnityEngine.Random.Range (0, workingList.Count);
					stageList.Add(workingList[randIndex]);
					workingList.RemoveAt (randIndex);
				}
				slicedList.Add(stageList);
			}
			return slicedList;
		}
	}
}

