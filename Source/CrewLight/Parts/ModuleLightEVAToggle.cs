/*
	This file is part of Crew Light /L Unleashed
		© 2021 Lisias T : http://lisias.net <support@lisias.net>
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

namespace CrewLight
{
	public class ModuleLightEVAToggle : PartModule
	{
		private List<Part> ogSymPart;
		private CL_GeneralSettings generalSettings;
//		private CL_EVALightSettings evaSettings;

		public override void OnStart (StartState state)
		{
			generalSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
//			evaSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_EVALightSettings> ();

			if ((part.Modules.Contains<ModuleLight> () || part.Modules.Contains ("ModuleKELight") 
				|| (part.Modules.Contains ("ModuleNavLight") && generalSettings.onAviationLights)) 
				&& generalSettings.useVesselLightsOnEVA) {
				ogSymPart = new List<Part> (part.symmetryCounterparts);
			} else {
				Destroy (this);
			}
		}

		[KSPEvent (active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiName = "Toggle Light")]
		public void LightToggleEVA ()
		{
			if (! generalSettings.lightSymLights) {
				// Remove the symmetry counter parts before lightning, then add them back
				part.symmetryCounterparts.Clear ();
			}

			if (part.Modules.Contains<ModuleLight> ()) {
				List<ModuleLight> lights = part.Modules.GetModules<ModuleLight> ();
				foreach (ModuleLight light in lights) {
					if (light.isOn) {
						SwitchLight.Off (light);
					} else {
						SwitchLight.On (light);
					}
				}
			}
			if (part.Modules.Contains ("ModuleKELight")) {
				foreach (PartModule partM in part.Modules) {
					if (partM.ClassName == "ModuleKELight") {
						if ((bool)partM.GetType ().InvokeMember ("isOn", System.Reflection.BindingFlags.GetField, null, partM, null)) {
							SwitchLight.Off (part);
						} else {
							SwitchLight.On (part);
						}
					}
				}
			}
			if (part.Modules.Contains ("ModuleNavLight")) {
				foreach (PartModule partM in part.Modules) {
					if (partM.ClassName == "ModuleNavLight") {
						if ((int)partM.GetType ().InvokeMember ("navLightSwitch", System.Reflection.BindingFlags.GetField, null, partM, null) != 0) {
							SwitchLight.Off (part);
						} else {
							SwitchLight.On (part);
						}
					}
				}
			}

			if (! generalSettings.lightSymLights) {
				part.symmetryCounterparts = ogSymPart;
			}
		}

	}
}

