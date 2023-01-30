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
using System;
using System.Collections.Generic;

namespace CrewLight.Support
{
	public interface Interface
	{
		string[] ModuleNames { get; }

		bool IsActive(PartModule pm);

		bool IsExternalLight(PartModule pm);

		bool IsAviationLight(PartModule pm);
		bool IsBeaconLight(PartModule pm);
		bool IsNavigationLight(PartModule pm);
		bool IsStrobeLight(PartModule pm);

		bool IsOn(PartModule pm);
		void TurnOn(PartModule pm);
		void TurnOff(PartModule pm);
	}

	internal interface PartInterface
	{
		bool IsActive();

		bool IsExternalLight();

		bool IsAviationLight();
		bool IsBeaconLight();
		bool IsNavigationLight();
		bool IsStrobeLight();

		bool IsOn();
		void TurnOn();
		void TurnOff();
	}

	internal class Facade
	{
		private class Dummy : Interface
		{
			private static readonly string[] moduleNames = new string[]{};
			string[] Interface.ModuleNames => moduleNames;

			bool Interface.IsActive(PartModule pm) => false;
			bool Interface.IsExternalLight(PartModule pm) => false;
			bool Interface.IsAviationLight(PartModule pm) => false;
			bool Interface.IsBeaconLight(PartModule pm) => false;
			bool Interface.IsNavigationLight(PartModule pm) => false;
			bool Interface.IsStrobeLight(PartModule pm) => false;

			bool Interface.IsOn(PartModule pm) => false;
			void Interface.TurnOff(PartModule pm) { }
			void Interface.TurnOn(PartModule pm) { }
		}
		private static readonly Dummy DUMMY = new Dummy();

		private static readonly Dictionary<string,Interface> modules = new Dictionary<string, Interface>();
		private static string[] moduleKeys = null;
		internal static string[] Modules => moduleKeys;

		private class PartImplementation : PartInterface
		{
			private readonly Part part;
			private readonly PartModule targetModule;
			private readonly Interface ifc;

			internal PartImplementation(Part part)
			{
				this.part = part;
				foreach (PartModule pm in part.Modules) if (Facade.IsSupported(pm))
				{
					this.targetModule = pm;
					this.ifc = Facade.Instance(pm);
					break;
				}
			}

			bool PartInterface.IsActive() => this.ifc.IsActive(this.targetModule);
			bool PartInterface.IsAviationLight() => this.ifc.IsAviationLight(this.targetModule);
			bool PartInterface.IsBeaconLight() => this.ifc.IsBeaconLight(this.targetModule);
			bool PartInterface.IsExternalLight() => this.ifc.IsExternalLight(this.targetModule);
			bool PartInterface.IsNavigationLight() => this.ifc.IsNavigationLight(this.targetModule);
			bool PartInterface.IsOn() => this.ifc.IsOn(this.targetModule);
			bool PartInterface.IsStrobeLight() => this.ifc.IsStrobeLight(this.targetModule);

			void PartInterface.TurnOff() => this.ifc.TurnOff(this.targetModule);
			void PartInterface.TurnOn() => this.ifc.TurnOn(this.targetModule);
		}
		internal static PartInterface Instance(Part part)
		{
			if (null == moduleKeys) initialize();
			return new PartImplementation(part);
		}

		internal static Interface Instance(PartModule pm)
		{
			if (null == moduleKeys) initialize();

			if(modules.ContainsKey(pm.moduleName)) return modules[pm.moduleName];
			return DUMMY;
		}

		internal static bool IsSupported(Part part)
		{
			if (null == moduleKeys) initialize();
			foreach (string m in moduleKeys) if (part.Modules.Contains(m))
				return true;
			return false;
		}

		internal static bool IsSupported(PartModule pm)
		{
			if (null == moduleKeys) initialize();
			int i = moduleKeys.IndexOf<string>(pm.moduleName);
			return i > -1;
		}

		private static void initialize()
		{
			foreach (Type t in KSPe.Util.SystemTools.Type.Search.By(typeof(Interface)))
			{
				Log.dbg("Found Interface client {0}", t.FullName);
				Interface o = (Interface)System.Activator.CreateInstance(t);
				foreach (string s in o.ModuleNames)
					modules[s] = o;
			}
			moduleKeys = new string[modules.Keys.Count];
			modules.Keys.CopyTo(moduleKeys, 0);
			Log.dbg("Suitable modules found: {0} {1}", moduleKeys.Length, string.Join(", ", moduleKeys));
		}
	}
}
