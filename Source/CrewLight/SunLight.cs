﻿/*
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
	public class SunLight : MonoBehaviour
	{

		private Vessel vessel;
		private List<PartModule> modulesLight;
		private bool inDark;

		private CL_SunLightSettings settings;
		private CL_GeneralSettings generalSettings;

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

			StartCoroutine ("StartSunLight");
		}

		[UsedImplicitly]
		private void OnDestroy ()
		{
			StopAllCoroutines ();
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
							SwitchLight.Instance.On(modulesLight);
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
					SwitchLight.Instance.Off(modulesLight);
					inDark = false;
				}
			} else {
				if (!inDark) {
					if (settings.useStaggeredLight) {
						StartCoroutine ("StageLight");
					} else {
						SwitchLight.Instance.On(modulesLight);
					}
					inDark = true;
				}
			}
		}

		private IEnumerator StartSunLight ()
		{
			yield return StartCoroutine ("FindLightPart");

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

		private IEnumerator StageLight ()
		{
			foreach (List<PartModule> stageList in SliceLightList ()) {
				SwitchLight.Instance.On(stageList);
				if (settings.useRandomDelay) {
					yield return new WaitForSeconds (UnityEngine.Random.Range (.4f, 2f));
				} else {
					yield return new WaitForSeconds (settings.delayStage);
				}
			}
		}

		private List<List<PartModule>> SliceLightList ()
		{
			List<List<PartModule>> slicedList = new List<List<PartModule>> ();
			List<PartModule> workingList = new List<PartModule> (modulesLight);

			while (workingList.Count != 0) {
				List<PartModule> stageList = new List<PartModule> ();
				int rndLightInStage = UnityEngine.Random.Range (settings.minLightPerStage, settings.maxLightPerStage);
				if (rndLightInStage > workingList.Count) {
					rndLightInStage = workingList.Count;
				}
				for (int i = 0 ; i < rndLightInStage ; i++) {
					int randIndex = UnityEngine.Random.Range (0, workingList.Count);
					stageList.Add (workingList [randIndex]);
					workingList.RemoveAt (randIndex);
				}
				slicedList.Add (stageList);
			}
			return slicedList;
		}

		private IEnumerator FindLightPart ()
		{
			modulesLight = new List<PartModule> ();

			int iSearch = -1;

			yield return new WaitForSeconds (.1f);

			foreach (Part part in vessel.Parts)
			{
				iSearch++;
				if (iSearch >= GameSettingsLive.maxSearch) {
					yield return new WaitForSeconds (.1f);
					iSearch = 0;
				}

				// Check if the part is a landing gear/wheel
				if (part.Modules.Contains<ModuleStatusLight> ()) {
					continue;
				}

				// Check if part is uncrewed
				if (part.CrewCapacity == 0 || ! generalSettings.useTransferCrew)
					foreach (PartModule partM in part.Modules)
						if (Support.Facade.Instance(partM).IsActive(partM))
						{
							if (settings.onlyNoAGpart && partM.Actions.Contains(KSPActionGroup.Light))
								continue;
							modulesLight.Add (partM);
						}
			}
		}
	}
}

