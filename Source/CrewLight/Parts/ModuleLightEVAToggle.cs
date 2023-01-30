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

namespace CrewLight
{
	public class ModuleLightEVAToggle : PartModule
	{
		private List<Part> ogSymPart;
		private CL_GeneralSettings generalSettings;

		public override void OnStart (StartState state)
		{
			this.enabled = true;

			generalSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
			if (generalSettings.useVesselLightsOnEVA)
			{
				Support.PartInterface light = Support.Facade.Instance(part);
				if (
					!light.IsNavigationLight()
					||
					(generalSettings.onAviationLights && light.IsNavigationLight())
				)
				{
					ogSymPart = new List<Part> (part.symmetryCounterparts);
					return;
				}
			}
			this.enabled = false;
		}

		[KSPEvent (active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiName = "Toggle Light")]
		public void LightToggleEVA ()
		{
			if (! generalSettings.lightSymLights) {
				// Remove the symmetry counter parts before lightning, then add them back
				part.symmetryCounterparts.Clear ();
			}

			foreach (PartModule partM in part.Modules)
			{ 
				if (Support.Facade.Instance(partM).IsOn(partM))
					Support.Facade.Instance(partM).TurnOff(partM);
				else
					Support.Facade.Instance(partM).TurnOn(partM);
			}

			if (!generalSettings.lightSymLights) {
				part.symmetryCounterparts = ogSymPart;
			}
		}

	}
}

