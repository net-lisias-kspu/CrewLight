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
using SwitchLights = LASL.KSP.Support.SwitchLights;

namespace CrewLight
{
	public class ModuleBeaconLightEngine : PartModule
	{
		private bool isOn = false;
		private bool IsBeaconLight = false;

		public override void OnStart (StartState state)
		{
			CL_AviationLightsSettings settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_AviationLightsSettings> ();
			if (! HighLogic.LoadedSceneIsFlight || ! settings.beaconOnEngine) {
				this.enabled = false;
				return;
			}

			this.enabled = this.IsBeaconLight = Registry.SwitchLights.Instance[this.part].IsBeaconLight();
		}

		[UsedImplicitly]
		private void FixedUpdate ()
		{
			if (part.vessel.ctrlState.mainThrottle > 0 && !this.isOn) {
				Registry.SwitchLights.Instance[this.part].TurnOn();
				this.isOn = true;
				return;
			} 
			if (part.vessel.ctrlState.mainThrottle == 0 && this.isOn){
				Registry.SwitchLights.Instance[this.part].TurnOff();
				this.isOn = false;
			}
		}
	}
}

