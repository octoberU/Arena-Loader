# Audica Arena Loader

A mod to load custom arenas for the game Audica.
Get the latest release [here](https://github.com/octoberU/Arena-Loader/releases/latest)
Get MelonLoader [here](https://github.com/HerpDerpinstine/MelonLoader/releases/latest)

## Installation

* Download the latest release and MelonLoader 0.2.5 or higher.
* Place the ArenaLoader.dll into `Audica\Mods`.
* Place all the downloaded arenas into `Audica\Mods\Arenas`.
* Custom arenas will be added to the options menu

## Creating your own arena

* Download [Unity 2018.4.6](https://unity3d.com/get-unity/download/archive)
* Get the Unity Project [here](https://drive.google.com/file/d/1yw6wXblnhMXYsovsKcvuuAQA_nQ0TmCP/view?usp=sharing)
* Add the project to your Unity Hub by selecting by extracting and selecting the project folder
* Duplicate ane of the scenes and use them as templates, modify the scene however you want.
* Add the scene to the asset labels with the name of your scene + `.arena`. Like [this](https://i.imgur.com/uxczfuz.png)
* Export the arena through "Arena/Export Arena" to a directory of your choice
* Place the exported `.arena` files in `Audica\Mods\Arenas`
* **The name of your asset bundle and the scene name has to match in order for the arena to be loaded**

If your arena doesn't load ingame, make sure you're using the right MelonLoader version and that you placed the arenas into the correct folder.
