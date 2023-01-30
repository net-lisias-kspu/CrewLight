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
using KSPe.Annotations;
using UnityEngine;

namespace CrewLight
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class DisableLightAG : MonoBehaviour
	{
		private bool disableCrewAG;
		private bool disableAllAG;

		[UsedImplicitly]
		private void Start ()
		{
			CL_GeneralSettings settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
			disableCrewAG = settings.disableCrewAG;
			disableAllAG = settings.disableAllAG;

			if (disableCrewAG || disableAllAG) {
				GameEvents.onEditorPartEvent.Add (CheckForLight);
			}
		}

		[UsedImplicitly]
		private void OnDestroy ()
		{
			if (disableCrewAG || disableAllAG) {
				GameEvents.onEditorPartEvent.Remove (CheckForLight);
			}
		}

		private void CheckForLight (ConstructionEventType constrE, Part part)
		{
			if (constrE == ConstructionEventType.PartCreated) {
				if (disableCrewAG && !disableAllAG) {
					if (part.CrewCapacity < 1) { return; }
				}
				
				if (Support.Facade.IsSupported(part))
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