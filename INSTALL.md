# CrewLight /L

**CrewLight** is an automatic light manager.

**CrewLight /L** is CrewLight under Lisias' maintenance.


## Installation Instructions

To install, place the GameData folder inside your Kerbal Space Program folder:

* **REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**, including any other fork:
	+ Delete `<KSP_ROOT>/GameData/CrewLight`
* Extract the package's `GameData/CrewLight` folder into your KSP's as follows:
	+ `<PACKAGE>/GameData/CrewLight` --> `<KSP_ROOT>/GameData/CrewLight`

The following file layout must be present after installation:

```
<KSP_ROOT>
	[GameData]
		[000_KSPAPIExtensions]
			...
		[CrewLight]
			[Localization]
				en_us.cfg
				...
			[MMPatch]
				KISWhiteList.cfg
				...
			CHANGE_LOG.md
			CrewLight.dll
			CrewLight.version
			LICENSE
			NOTICE
			README.md
		000_KSPe.dll
		ModuleManager.dll
		...
	KSP.log
	PartDatabase.cfg
	...
```

### Dependencies

* [KSPe](https://github.com/net-lisias-ksp/KSPAPIExtensions/releases/) (for KSP >= 1.2.2 - yeah, anything goes)
	+ Not Included
* [Module Manager](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-*) 3.0.7 or later
	+ Not Included
