﻿using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using MelonLoader;
using ArenaLoader;
using UnityEngine.SceneManagement;

namespace AudicaModding
{
    internal static class Hooks
    {
        private static int buttonCount = 0;
        private static bool addedEnv = false;

        [HarmonyPatch(typeof(OptionsMenu), "AddEnvButton", new Type[] { typeof(int), typeof(string), typeof(string)})]
        private static class AddEnvButton
        {
            private static void Postfix(OptionsMenu __instance, int col, string buttonLabel, string envString)
            {
                if(envString == "environment5" && !addedEnv)
                {
                    int column = 1;
                    for (int i = 0; i < ArenaLoaderMod.arenaNames.Count; i++)
                    {
                        __instance.AddEnvButton(column, ArenaLoaderMod.arenaNames[i], ArenaLoaderMod.arenaNames[i]);
                        if (column == 1)
                            column = 0;
                        else
                            column = 1;
                    }
                    addedEnv = true;
                }
            }
        }

        [HarmonyPatch(typeof(OptionsMenu), "AddButton", new Type[] { typeof(int), typeof(string), typeof(OptionsMenuButton.SelectedActionDelegate), typeof(OptionsMenuButton.IsCheckedDelegate), typeof(string), typeof(OptionsMenuButton), })]
        private static class AddButtonButton
        {
            private static void Postfix(OptionsMenu __instance, int col, string label, OptionsMenuButton.SelectedActionDelegate onSelected, OptionsMenuButton.IsCheckedDelegate isChecked)
            {
                if (__instance.mPage == OptionsMenu.Page.Main)
                {
                    buttonCount++;
                    addedEnv = false;
                    if (buttonCount == 9)
                    {
                        int column = col;
                        if (column == 1)
                            column = 0;
                        else
                            column = 1;
                        ArenaLoaderUI.AddArenaCustomizationButton(__instance, column);
                    } 
                }
            }
        }

        [HarmonyPatch(typeof(OptionsMenu), "ShowPage", new Type[] { typeof(OptionsMenu.Page) })]
        private static class ResetButtons
        {
            private static void Prefix(OptionsMenu __instance, OptionsMenu.Page page)
            {
                addedEnv = false;
                buttonCount = 0;
            }
        }


        [HarmonyPatch(typeof(OptionsMenu), "BackOut", new Type[0])]
        private static class Backout_AL
        {
            private static void Postfix(OptionsMenu __instance)
            {
                addedEnv = false;
            }
        }

        [HarmonyPatch(typeof(CampaignStructure), "IsUnlocked", new Type[] { typeof(CampaignStructure.UnlockType), typeof(string)})]
        private static class CheckforLock
        {
            private static void Postfix(CampaignStructure __instance, CampaignStructure.UnlockType unlockType, string unlockName, ref bool __result)
            {
                if (ArenaLoaderMod.arenaNames.Contains(unlockName))
                {
                    __result = true;
                }
            }
        }

    }
}
