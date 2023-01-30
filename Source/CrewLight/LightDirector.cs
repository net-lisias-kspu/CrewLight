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
using KSPe.Annotations;
using UnityEngine;

namespace CrewLight
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class LightDirector : MonoBehaviour
	{
		private CL_GeneralSettings generalSettings;
		private CL_SunLightSettings sunLightSettings;
//		private CL_HeadlightSettings headLightSettings;

		[UsedImplicitly]
		private void Start ()
		{
			this.RegisterEventHandlers ();

			generalSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
			sunLightSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_SunLightSettings> ();
//			headLightSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_HeadlightSettings> ();
			// CrewLight :
			if (generalSettings.useTransferCrew) {
				GameEvents.onCrewTransferred.Add (CrewLightTransfer);
				GameEvents.onVesselChange.Add (CrewLightVessel);
				CrewLightVessel (FlightGlobals.ActiveVessel);
			}

			// EVALight :
			if (generalSettings.useSunLightEVA) {
				GameEvents.onCrewOnEva.Add (SunLightEVA);
				Vessel vessel = FlightGlobals.ActiveVessel;
				if (vessel.isEVA) {
					SunLightEVA (vessel, vessel.FindPartModulesImplementing<KerbalEVA> () [0]);
				}
			}

			// MorseLight :
			if (generalSettings.useMorseCode) {
				GameEvents.onVesselLoaded.Add (MorseLightAddVessel);
			}

			// SunLight :
			if (sunLightSettings.useSunLight) {
				GameEvents.onVesselGoOffRails.Add (SunLightAddVessel);
				GameEvents.onVesselCreate.Add (SunLightAddVessel);
				GameEvents.onVesselPartCountChanged.Add (SunLightVesselChanged);
			}
		}

		[UsedImplicitly]
		private void OnDestroy ()
		{
			// CrewLight :
			if (generalSettings.useTransferCrew) {
				GameEvents.onCrewTransferred.Remove (CrewLightTransfer);
				GameEvents.onVesselChange.Remove (CrewLightVessel);
			}

			// EVALight :
			if (generalSettings.useSunLightEVA) {
				GameEvents.onCrewOnEva.Remove (SunLightEVA);
			}

			// MorseLight :
			if (generalSettings.useMorseCode) {
				GameEvents.onVesselLoaded.Remove (MorseLightAddVessel);
			}

			// SunLight :
			if (sunLightSettings.useSunLight) {
				GameEvents.onVesselGoOffRails.Remove (SunLightAddVessel);
				GameEvents.onVesselCreate.Remove (SunLightAddVessel);
				GameEvents.onVesselPartCountChanged.Remove (SunLightVesselChanged);
			}
		}

		#region CrewLight

		private void CrewLightVessel (Vessel vessel)
		{
			StartCoroutine ("CrewLightVesselRoutine", vessel);
		}

		private IEnumerator CrewLightVesselRoutine (Vessel vessel)
		{
			yield return new WaitForSeconds (.1f);

			if (vessel.crewedParts != 0 && vessel.isEVA == false) {
				foreach (ProtoCrewMember crewMember in vessel.GetVesselCrew()){
					if (crewMember.KerbalRef != null) {// If this is false it should means the Kerbal is in a Command Seat
						SwitchLight.On (crewMember.KerbalRef.InPart);
					}
				}
			}
		}

		private void CrewLightTransfer (GameEvents.HostedFromToAction<ProtoCrewMember, Part> eData)
		{
			SwitchLight.On (eData.to);
			if (eData.from.protoModuleCrew.Count == 0) {
				SwitchLight.Off (eData.from);
			}
		}
			
		#endregion

		#region EVALight

		private void SunLightEVA (GameEvents.FromToAction<Part, Part> eData)
		{
			if (eData.to.Modules.Contains<KerbalEVA> ())
			{
				SunLightEVA (eData.from.vessel, eData.to.Modules.GetModule<KerbalEVA> ());
			}
		}

		private void SunLightEVA (Vessel vessel, KerbalEVA kerbal)
		{
			if (generalSettings.onForEVASpace && (vessel.situation == Vessel.Situations.ESCAPING 
				|| vessel.situation == Vessel.Situations.FLYING 
				|| vessel.situation == Vessel.Situations.ORBITING 
				|| vessel.situation == Vessel.Situations.SUB_ORBITAL)) {

				kerbal.lampOn = true;
				return;
			}
			if (generalSettings.onForEVALanded && (vessel.situation == Vessel.Situations.LANDED 
				|| vessel.situation == Vessel.Situations.PRELAUNCH 
				|| vessel.situation == Vessel.Situations.SPLASHED)) {

				kerbal.lampOn = true;
				return;
			}

			bool isSunShine = false;
			RaycastHit hit;
			Vector3d vesselPos = vessel.GetWorldPos3D ();
			Vector3d sunPos = FlightGlobals.GetBodyByName ("Sun").position;
			if (Physics.Raycast(vesselPos, sunPos, out hit, Mathf.Infinity, GameSettingsLive.layerMask)) {
				if (hit.transform.name == "Sun") {
					isSunShine = true;
				}
			}

			if (!isSunShine) {
				kerbal.lampOn = true;
			}
		}

		#endregion

		#region MorseLight

		private void MorseLightAddVessel (Vessel vessel)
		{
			if (vessel != FlightGlobals.ActiveVessel) {
				vessel.gameObject.AddOrGetComponent<MorseLight> ();
			}
		}

		#endregion

		#region SunLight

		private void SunLightVesselChanged (Vessel vessel)
		{
			// If a vessel's part count changed then delete all instance of SunLight
			// on this vessel and add a new one, that will automatically search all 
			// parts of the vessel for lightable one
			if (vessel.GetComponent<SunLight> () != null) {
				foreach (Part part in vessel.Parts) {
					if (part.GetComponent<SunLight> () != null) {
						Destroy (part.GetComponent<SunLight> ());
					}
				}
				vessel.gameObject.AddComponent<SunLight> ();
			}
		}

		private void SunLightAddVessel (Vessel vessel)
		{
			if (vessel.loaded) {
				vessel.gameObject.AddOrGetComponent<SunLight> ();
			}
		}

		#endregion

		private void OnVesselCreated(object vessel)
		{
			Vessel v = (Vessel)vessel;
			Registry.Vessels.CheckAndAdd (v);
		}

		private void OnVesselDestroyed (object vessel)
		{
			Vessel v = (Vessel)vessel;
			Registry.Vessels.RemoveIfExists (v);
		}

		private void RegisterEventHandlers()
		{
			GameEvents.onVesselCreate.Add (this.OnVesselCreated);	
			GameEvents.onVesselDestroy.Add (this.OnVesselDestroyed);	
		}

	}
}

