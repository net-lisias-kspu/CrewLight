# CrewLight /L :: Archive

* 2017-0922: 1.9 (Li0n) for KSP 1.3.0
	+ Fixes for Aviation Lights
	+ The beacon lights flashing when the throttle is up is working again
	+ AviationLights lights can now be toggle on EVA, no blinking option tho, they'll use the blinking preset defined in the settings.cfg
	+ Fail-safe for part using AviationLights module with a different name than in the original mod
* 2017-0921: 1.8 (Li0n) for KSP 1.3.0
	+ support for [Ven's Stock Part Revamp](https://forum.kerbalspaceprogram.com/index.php?/topic/83696-122-stock-part-revamp-update-196-released-source-files/)
* 2017-0919: 1.7 (Li0n) for KSP 1.3.0
	+ support for [KerbalElectric](https://forum.kerbalspaceprogram.com/index.php?/topic/165449-wip-kerbal-electric-moar-lights/)
	+ fix for the "disableAllAg" setting didn't do anything
	+ bundle ModuleManager 2.8.1
* 2017-0529: 1.6 (Li0n) for KSP 1.3.0
	+ recompile for KSP 1.3.0
	+ update ModuleManager to 2.8.0
* 2017-0320: 1.5 (Li0n) for KSP 1.2.2
	+ fix for the "night lights" list not being fully populated if a landing gear was added to the ship
* 2017-0217: 1.4 (Li0n) for KSP 1.2.2
	+ Enhanced support for Aviation Light by BigNose / MoarDV, each light can have their lightning style define in the config, beacon light will go on only when you push the throttle (configurable)
	+ fix for light going off while a part is still crewed
	+ better method for lightning part, no more "SendMessage()", using cast and reflection instead
	+ fix a typo in the settings file
* 2017-0213: 1.3 (Li0n) for KSP 1.2.2
	+ Fix for "Electric Lights" by Alshain
* 2017-0204: 1.2 (Li0n) for KSP 1.2.2
	+ Light triggered by the sun are now lighted in a staggered way, can be disabled/tweaked in settings.cfg
	+ Some quality of life improvement of the code
* 2017-0114: 1.1 (Li0n) for KSP 1.2.2
	+ Adjust the delay between two check of the sun position according to the warp-time speed
		- Settings.cfg : delay_in_low_timewarp (value get divided by the current time-warp speed) and delay_in_high_timewarp
	+ Change the default settings for the morse code, light will stay on longer
	+ Toggling a vessel light on EVA will no longer toggle its symmetry counter parts
		- Can be changed in the settings
		- Also add a setting to disable light interaction on EVA
	+ Add a setting to disable lightning of a part when a crew is inside, if used crewed part 's light will be toggled as the sun rises / fall
	+ Enhance settings behavior, adding new settings no longer force the whole settings.cfg file to be reset
* 2017-0112: 1.0 (Li0n) for KSP 1.2.2
	+ For KIS : white-list ModuleLightEVAToggle so lights can be stock-pile again
	+ Update readme on [Github](https://github.com/Li0n-0/CrewLight/releases) and [SpaceDock](http://spacedock.info/mod/1012/Crew%20Light)
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
