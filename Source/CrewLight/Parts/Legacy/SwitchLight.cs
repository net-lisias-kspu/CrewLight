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
using System.Collections.Generic;

// FIXME: Why this thingy is on Legacy directory?
namespace CrewLight
{
	public class SwitchLight
	{
		private static SwitchLight instance = null;
		public static SwitchLight Instance = instance??(instance = new SwitchLight());

		private readonly CL_GeneralSettings generalSettings;
		private readonly CL_AviationLightsSettings aviationLSettings;

		private SwitchLight()
		{
			generalSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
			aviationLSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_AviationLightsSettings> ();
		}

		public void On(PartModule light) => Support.Facade.Instance(light).TurnOn(light);
		public void Off(PartModule light) => Support.Facade.Instance(light).TurnOff(light);
		public void On(Part part) => this.On(this.GetLightModule(part));
		public void Off(Part part) => this.Off(this.GetLightModule(part));

		public void On(List<PartModule> modulesLight)
		{
			foreach (PartModule light in modulesLight)
				On(light);
		}

		public void Off(List<PartModule> modulesLight)
		{
			foreach (PartModule light in modulesLight)
				Off(light);
		}

		public bool IsOn(Part part) => IsOn(this.GetLightModule(part));
		public static bool IsOn(List<PartModule> modulesLight) => IsOn(modulesLight[0]); // not that usefull but needed for IsOn(Part)
		public static bool IsOn(PartModule light) => Support.Facade.Instance(light).IsOn(light);

		internal List<PartModule>GetLightModule(Part part)
		{
			List<PartModule> lightList = new List<PartModule> ();

			foreach (PartModule module in part.Modules) if (Support.Facade.Instance(module).IsActive(module))
				lightList.Add (module);

			return lightList;
		}
	}
}

