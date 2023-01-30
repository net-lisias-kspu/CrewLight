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
using TargetModule = global::AviationLights.ModuleNavLight;

namespace CrewLight.Support.AviationLights
{
	public class AviationLights : CrewLight.Support.Interface
	{
		private readonly CL_AviationLightsSettings aviationLightsSettings;
		public AviationLights ()
		{
			this.aviationLightsSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_AviationLightsSettings> ();
		}

		private static string[] moduleNames = new string[] { "ModuleNavLight" };
		string[] Interface.ModuleNames => moduleNames;

		bool Interface.IsActive(PartModule pm) => !aviationLightsSettings.beaconOnEngine && !(this as Interface).IsBeaconLight(pm);

		bool Interface.IsExternalLight(PartModule pm) => true;

		bool Interface.IsAviationLight(PartModule pm) => true;
		bool Interface.IsBeaconLight(PartModule pm)
		{
			bool r = pm.part.name.StartsWith("lightbeacon_");
			r = r || this.IsFromPreset("beacon", pm);
			return r;
		}

		bool Interface.IsNavigationLight(PartModule pm)
		{
			bool r = pm.part.name.StartsWith("lightnav_");
			r = r || this.IsFromPreset("nav", pm);
			return r;
		}

		bool Interface.IsStrobeLight(PartModule pm)
		{
			bool r = pm.name.StartsWith("lightstrobe_");
			r = r || this.IsFromPreset("strobe", pm);
			return r;
		}

		private bool IsFromPreset(string name, PartModule pm)
		{
			TargetModule tm = (pm as TargetModule);
			return name.Equals(tm.typePreset);
		}

		bool Interface.IsOn(PartModule pm)
		{
			TargetModule tm = (pm as TargetModule);
			Log.dbg("Part Module {0} is {1}", tm, tm.navLightSwitch);
			return 0 != tm.navLightSwitch;
		}

		void Interface.TurnOn(PartModule pm)
		{
			TargetModule tm = (pm as TargetModule);
			tm.navLightSwitch = tm.toggleModeSelector;
			Log.dbg("Part Module {0} was turned on", tm);
		}

		void Interface.TurnOff(PartModule pm)
		{
			TargetModule tm = (pm as TargetModule);
			tm.navLightSwitch = 0;
			Log.dbg("Part Module {0} was turned off", tm);
		}
	}
}
