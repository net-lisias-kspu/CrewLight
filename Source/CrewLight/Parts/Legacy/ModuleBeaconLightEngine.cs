/*
	This file is part of Crew Light
	© 2021 Lisias T : http://lisias.net <support@lisias.net>

	CrewLight is double licensed, as follows:

	* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
	* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	KSP-Recall is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with KSP-Recall. If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with KSP-Recall. If not, see <https://www.gnu.org/licenses/>.

*/
using System;
using UnityEngine;

namespace CrewLight
{
	public class ModuleBeaconLightEngine : PartModule
	{
		private PartModule navLight;
		private bool isOn = false;

		public override void OnStart (StartState state)
		{
			CL_AviationLightsSettings settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_AviationLightsSettings> ();
			if (! HighLogic.LoadedSceneIsFlight || ! settings.beaconOnEngine) {
				Destroy (this);
			}

			foreach (PartModule pm in part.Modules) {
				if (pm.ClassName == "ModuleNavLight") {
					navLight = pm;
					break;
				}
			}
		}

		public void FixedUpdate ()
		{
			if (part.vessel.ctrlState.mainThrottle > 0 && ! isOn) {
				SwitchLight.On (navLight);
				isOn = true;
				return;
			} 
			if (part.vessel.ctrlState.mainThrottle == 0 && isOn){
				SwitchLight.Off (navLight);
				isOn = false;
			}
		}
	}
}

