using MelonLoader;
using System;
using UnityEngine;

namespace ArenaLoader
{
	internal static class ArenaLoaderUI
	{
		static public void AddArenaCustomizationButton(OptionsMenu optionsMenu, int col)
		{
			optionsMenu.AddButton(col, "Customize Arena", new Action(() => {
				GoToArenaPage(optionsMenu);
				}), null, "Customize your current arena.");
		}

		public static void GoToArenaPage(OptionsMenu optionsMenu)
		{
			optionsMenu.ShowPage(OptionsMenu.Page.Gameplay);
			CleanUpPage(optionsMenu);
			AddButtons(optionsMenu);
			optionsMenu.screenTitle.text = "Customize Arena";
		}

		private static void AddButtons(OptionsMenu optionsMenu)
		{
			optionsMenu.AddHeader(0, "Skybox controls");

			var RotationSlider = optionsMenu.AddSlider(0, "Skybox Rotation", "P",
				new Action<float>(x => { ArenaLoaderMod.RotateSkybox(x * 5); }),
				null);
			RotationSlider.label.text = "Rotation";

			var ExposureSlider = optionsMenu.AddSlider(1, "Skybox Exposure", "P",
			new Action<float>(x => { ArenaLoaderMod.ChangeExposure(x * 0.05f); }),
			null);
			ExposureSlider.label.text = "Brightness";

			var ReflectionSlider = optionsMenu.AddSlider(0, "Skybox Reflection", "P",
			new Action<float>(x => { ArenaLoaderMod.ChangeReflectionStrength(x * 0.05f); }),
			null);
			ReflectionSlider.label.text = "Reflection";

			optionsMenu.AddTextBlock(0, "These settings will reset when you enter a new arena. A way to save current arena settings will be added in a future update");

			optionsMenu.AddHeader(0, "Custom skybox");
            optionsMenu.AddSlider(0, "Custom Skybox", null, new Action<float>((amount) => { ArenaLoaderMod.skyboxLoader.Index += (int)amount;}), new Func<float>(() => { return (float)ArenaLoaderMod.skyboxLoader.Index; }), new Action(() => { ArenaLoaderMod.skyboxLoader.Index = 0; }), "Skybox to load", new Func<float, string>((amount) => 
			{
                if(ArenaLoaderMod.skyboxLoader.skyboxes.Count == 0) return "Skybox folder is empty";
				Material currentSkybox = ArenaLoaderMod.skyboxLoader.skyboxes[ArenaLoaderMod.skyboxLoader.Index];
				if (currentSkybox != null) return currentSkybox.name;
				else return "Skybox folder is empty";
			}), optionsMenu.sliderCustomModelPrefab);
			optionsMenu.AddButton(0, "Apply skybox", new Action(() =>
			{
				if (ArenaLoaderMod.skyboxLoader.skyboxes.Count == 0) return;
				var newSkybox = ArenaLoaderMod.skyboxLoader.skyboxes[ArenaLoaderMod.skyboxLoader.Index];
				if (newSkybox != null) ArenaLoaderMod.UpdateSkybox(newSkybox);
			}), null, "Apply the currently selected skybox");

			optionsMenu.AddButton(0, "Reload skybox folder", new Action(() =>
			{
				ArenaLoaderMod.skyboxLoader.LoadSkyboxes();
			}), null, "Deletes the currently loaded skyboxes and reloads the folder.\n<color=red>Only use this for working on skyboxes</color>");
			optionsMenu.AddTextBlock(0, "Create your own custom skyboxes from images in Audica\\Mods\\Arenas\\Skyboxes\nTo reset a custom skybox, load a different arena.");
        }

		private static void CleanUpPage(OptionsMenu optionsMenu)
		{
			Transform optionsTransform = optionsMenu.transform;
			for (int i = 0; i < optionsTransform.childCount; i++)
			{
				Transform child = optionsTransform.GetChild(i);
				if (child.gameObject.name.Contains("(Clone)"))
				{
					GameObject.Destroy(child.gameObject);
				}
			}
			optionsMenu.mRows.Clear();
			optionsMenu.scrollable.ClearRows();
			optionsMenu.scrollable.mRows.Clear();
		}
	}
}