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
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class DisableLightAG : MonoBehaviour
	{
		private bool disableCrewAG;
		private bool disableAllAG;

		void Start ()
		{
			CL_GeneralSettings settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
			disableCrewAG = settings.disableCrewAG;
			disableAllAG = settings.disableAllAG;

			if (disableCrewAG || disableAllAG) {
				GameEvents.onEditorPartEvent.Add (CheckForLight);
			}
		}

		void OnDestroy ()
		{
			if (disableCrewAG || disableAllAG) {
				GameEvents.onEditorPartEvent.Remove (CheckForLight);
			}
		}

		void CheckForLight (ConstructionEventType constrE, Part part)
		{
			if (constrE == ConstructionEventType.PartCreated) {
				if (disableCrewAG && !disableAllAG) {
					if (part.CrewCapacity < 1) { return; }
				}
				if (part.Modules.Contains<ModuleColorChanger> () 
					|| part.Modules.Contains<ModuleLight> () 
					|| part.Modules.Contains<ModuleAnimateGeneric> () 
					|| part.Modules.Contains ("WBILight")
					|| part.Modules.Contains ("ModuleKELight"))
				{
					foreach (PartModule partM in part.Modules) {
						if (partM.Actions.Contains(KSPActionGroup.Light)) {
							foreach (BaseAction action in partM.Actions) {
								if (action.actionGroup == KSPActionGroup.Light) {
									action.actionGroup = KSPActionGroup.None;
									break;
								}
							}
						}
					}
				}
			}
		}
	}
}