using System.Diagnostics;
using KSPe.Util.Log;

namespace CrewLight
{
	internal static class Log
	{
		private static readonly Logger log = Logger.CreateForType<Startup> ();

		internal static void force (string msg, params object [] @params)
		{
			log.force (msg, @params);
		}

		internal static void info (string msg, params object [] @params)
		{
			log.info (msg, @params);
		}

		internal static void warn (string msg, params object [] @params)
		{
			log.warn (msg, @params);
		}

		internal static void detail (string msg, params object [] @params)
		{
			log.detail (msg, @params);
		}

		internal static void error (string msg, params object [] @params)
		{
			log.error (msg, @params);
		}

		[ConditionalAttribute ("DEBUG")]
		internal static void dbg (string msg, params object [] @params)
		{
			log.trace (msg, @params);
		}
	}
}
