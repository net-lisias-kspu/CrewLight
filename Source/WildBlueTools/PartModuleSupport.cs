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
using TargetModule = global::WildBlueIndustries.WBILight;

namespace CrewLight.Support.WildBlueIndustries
{
	public class WildBlueTools : CrewLight.Support.Interface
	{
		public WildBlueTools () { }

		private static string[] moduleNames = new string[] { "WBILight" };
		string[] Interface.ModuleNames => moduleNames;

		bool Interface.IsActive(PartModule pm) => true;
		bool Interface.IsExternalLight(PartModule pm) => true;
		bool Interface.IsAviationLight(PartModule pm) => false;
		bool Interface.IsBeaconLight(PartModule pm) => false;
		bool Interface.IsNavigationLight(PartModule pm) => false;
		bool Interface.IsStrobeLight(PartModule pm) => false;

		bool Interface.IsOn(PartModule pm)
		{
			TargetModule tm = (pm as TargetModule);
			Log.dbg("Part Module {0} is {1}", tm, tm.isDeployed);
			return tm.isDeployed;
		}

		void Interface.TurnOn(PartModule pm)
		{
			TargetModule tm = (pm as TargetModule);
			tm.TurnOnLights();
			Log.dbg("Part Module {0} was turned on", tm);
		}

		void Interface.TurnOff(PartModule pm)
		{
			TargetModule tm = (pm as TargetModule);
			tm.TurnOffLights();
			Log.dbg("Part Module {0} was turned off", tm);
		}
	}
}
