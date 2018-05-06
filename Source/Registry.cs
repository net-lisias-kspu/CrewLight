using System;
using System.Collections;
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
