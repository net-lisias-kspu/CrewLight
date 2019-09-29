# CrewLight /L :: Archive

* 2016-1228: 0.10 (Li0n) for KSP 1.2.2
	+ New Function : Kerbal on EVA can toggle light (only for non-crewable part)
	+ Now support SurfaceLight by Why485, maintained by IgorZ
	+ Landing gear/wheel 's light no longer respond to the sun rise / fall
	+ ModuleManager by Sarbian is now required for toggling light on EVA
	+ ModuleManager is bundled
* 2016-1223: 0.9 (Li0n) for KSP 1.2.2
	+ Lights on Kerbal's helmet are now toggled as the sun rise/fall, can be tweaked in settings.cfg so lights are always on when in space or landed
	+ Better handling of the sunlight feature, mostly for building / modifying vessel with KIS
	+ Correct a typo in the .version file, KSP-AVC should now give the right link when a new version comes up
* 2016-1213: 0.8 (Li0n) for KSP 1.2.2
	+ Lights who respond to the sun rise/fall will now be toggled when reaching a certain depth
	+ Detection of terrain and KSC building shadow when checking if the sun shines on the craft
	+ Nearby vessels get theirs lights toggled as the sun rise/fall
	+ Added a .version file for [KSP-AVC](http://forum.kerbalspaceprogram.com/index.php?/topic/72169-12-ksp-avc-add-on-version-checker-plugin-1162-miniavc-ksp-avc-online-2016-10-13/)
	+ Remove source from the download, only on [GitHub](https://github.com/Li0n-0/CrewLight) now
* 2016-1209: 0.7 (Li0n) for KSP 1.2.2
	+ New function : Lights are toggled depending if the sun shine on the vessel or not, only for part without crew capacity and not in the Light action group (tweakable in settings.cfg), thanks Real-Gecko for the idea and advice
	+ New function : All lights can be removed from the light action group when building a vessel (so they all get toggled by the sun light), off by default
	+ Rebuild for KSP 1.2.2
	+ Part with multiple light are now correctly lighted, support for MK1 Cabin Hatch by skalou
	+ Use REGEX to determine if an animation is light related, support for MK1 Cabin Hatch by skalou
	+ Cleaned up and readability enhancement
* 2016-1129: 0.6 (Li0n) for KSP 1.2.1
	+ If you are updating this mod please DELETE THE OLD VERSION
			- Morse Code : nearby vessel will send you a morse message by toggling theirs lights as you approach
			- Settings File : GameData/CrewLight/PluginData/Settings.cfg option to disabled the morse message and customize it and disabling the auto removal of crewable part from the Light action group
			- Remove ModuleManager and all the patch
	+ If you are updating this mod please DELETE THE OLD VERSION
* 2016-1103: 0.5 (Li0n) for KSP 1.2.1
	+ KSP 1.2.1 recompile
	+ Add support for all the Wild Blue Industries mods (M.O.L.E. Buffalo, etc...)
	+ Enhanced support of the passengers cab from USI Karibou
	+ Enhanced support of the Orca command module from USI Freight Transport Technologies
		- New functionality : Toggle Lights action from all crewable parts are no longer automatically put in the "Light" action group. Only applied to newly assembled ship.
	+ From now on ModuleManager (by sarbian) is a requirement and is bundled
* 2016-1025: 0.4 (Li0n) for KSP 1.2
	+ Add support for Near Future  Spacecraft, thanks to Boomerang
* 2016-1023: 0.3 (Li0n) for KSP 1.2
	+ Code cleanup, thanks to Nereid
	+ Kerbals now turn lights off when they leave theirs pod
* 2016-1021: 0.2 (Li0n) for KSP 1.2
	+ Clean up some code, thanks to Diazo
* 2016-1021: 0.1 (Li0n) for KSP 1.2
	+ No changelog provided
