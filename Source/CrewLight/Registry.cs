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

namespace CrewLight
{
	namespace Registry {
		internal static class Vessels
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

		internal static class SwitchLights
		{
			private static LASL.KSP.Support.SwitchLights.Controller _instance = null;
			internal static LASL.KSP.Support.SwitchLights.Controller Instance => _instance ?? (_instance = new LASL.KSP.Support.SwitchLights.Controller(new Settings()));

			internal class Settings : LASL.KSP.Support.SwitchLights.ISettings, GameSettingsLive.IUpdateable
			{
				private int _maxPartsToUse = 200;
				private float _thresholdInSecs = 0;

				int LASL.KSP.Support.SwitchLights.ISettings.maxPartsToUse => _maxPartsToUse;
				float LASL.KSP.Support.SwitchLights.ISettings.thresholdInSecs => _thresholdInSecs;

				internal Settings()
				{
					GameSettingsLive.updateables.Add(this);
				}
				~Settings() { }
				void System.IDisposable.Dispose()
				{
					GameSettingsLive.updateables.Remove(this);
				}

				void GameSettingsLive.IUpdateable.Update()
				{
					{
						CL_AviationLightsSettings settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_AviationLightsSettings> ();
						this._maxPartsToUse = settings.maxSearch;
					}

					{ 
						CL_GeneralSettings settings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings> ();
						this._thresholdInSecs = Math.Min(settings.dahDuration, settings.ditDuration);
						this._thresholdInSecs = Math.Min(this._thresholdInSecs, settings.letterSpaceDuration);
					}
				}
			}
		}
	}
}
