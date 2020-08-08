using System;
using UnityEngine;

namespace AudicaModding
{
	internal static class ArenaLoaderUI
	{
		static public void AddArenaCustomizationButton(OptionsMenu optionsMenu, int col)
		{
			optionsMenu.AddButton(col, "Customize Arena", new System.Action(() => {
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
			
			var RotationSlider =  optionsMenu.AddSlider(0, "Skybox Rotation", "P",
				new Action<float>(x => { AudicaMod.RotateSkybox(x * 5); }),
				null);
			RotationSlider.label.text = "Rotation";
			
			var ExposureSlider = optionsMenu.AddSlider(1, "Skybox Exposure", "P",
			new Action<float>(x => { AudicaMod.ChangeExposure(x * 0.05f); }),
			null);
			ExposureSlider.label.text = "Brightness";

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