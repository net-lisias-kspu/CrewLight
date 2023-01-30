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

namespace CrewLight
{
	public class ModuleBeaconLightEngine : PartModule
	{
		private PartModule navLight;
		private bool isOn = false;

		public override void OnStart (StartState state)
		{
			this.enabled = true;

			CL_AviationLightsSettings settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_AviationLightsSettings> ();
			if (! HighLogic.LoadedSceneIsFlight || ! settings.beaconOnEngine) {
				this.enabled = false;
			}

			foreach (PartModule pm in part.Modules)
				if (Support.Facade.Instance(pm).IsBeaconLight(pm))
				{
					navLight = pm;
					break;
				}
		}

		[UsedImplicitly]
		private void FixedUpdate ()
		{
			if (part.vessel.ctrlState.mainThrottle > 0 && ! isOn) {
				Support.Facade.Instance(navLight).TurnOn(navLight);
				isOn = true;
				return;
			} 
			if (part.vessel.ctrlState.mainThrottle == 0 && isOn){
				Support.Facade.Instance(navLight).TurnOff(navLight);
				isOn = false;
			}
		}
	}
}

