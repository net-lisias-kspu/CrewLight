using System.Collections.Generic;
using UnityEngine;

namespace CrewLight
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private void Start()
		{
			Log.init();
			Log.force("Version {0}", Version.Text);
		}
	}

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
