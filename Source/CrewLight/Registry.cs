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
using System.Collections.Generic;

namespace CrewLight
{
	namespace Registry {
		public static class Vessels
		{
			private static HashSet<Vessel> knownVessels = new HashSet<Vessel> ();

			public static void CheckAndAdd(Vessel v)
			{
				if (knownVessels.Contains (v)) return;
				// Search for a Part that implements our interface. If found,
				//  this.knownVessels.Add (v);
				// else
				//  Instantiate by brute force a part and attach to it. (how? RiP).
			}

			public static void RemoveIfExists(Vessel v)
			{
				if (knownVessels.Contains (v)) knownVessels.Remove (v);
			}
		}

	}
}
