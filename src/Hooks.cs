﻿using Harmony;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace AudicaModding
{
    internal static class Hooks
    {
        public static void ApplyHooks(HarmonyInstance instance)
        {
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static bool addedEnv = false;

        [HarmonyPatch(typeof(OptionsMenu), "AddEnvButton", new Type[] { typeof(int), typeof(string), typeof(string) })]
        private static class AddEnvButton
        {
            private static void Postfix(OptionsMenu __instance, int col, string buttonLabel, string envString)
            {
                if(buttonLabel == "The Gate" && !addedEnv)
                {
                    int column = 1;
                    for (int i = 0; i < AudicaMod.arenaNames.Length; i++)
                    {
                        OptionsMenu.I.AddEnvButton(column, AudicaMod.arenaNames[i], AudicaMod.arenaNames[i]);
                        if (column == 1)
                            column = 0;
                        else
                            column = 1;
                    }
                    addedEnv = true;
                }
            }
        }

        [HarmonyPatch(typeof(CampaignStructure), "IsUnlocked", new Type[] { typeof(CampaignStructure.UnlockType), typeof(string)})]
        private static class CheckforLock
        {
            private static void Postfix(CampaignStructure __instance, CampaignStructure.UnlockType unlockType, string unlockName, ref bool __result)
            {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(EnvironmentLoader), "SwitchEnvironment", new Type[0])]
        private static class AdjustBloom
        {
            private static void Postfix(EnvironmentLoader __instance)
            {
                string nextenv = PlayerPrefs.GetString("environment_name");
                if (!defaultEnvironments.Contains(nextenv))
                {
                    BloomTweak();
                }
            }
        }

        static void BloomTweak()
        {
            PostprocController postproc = UnityEngine.Object.FindObjectOfType<PostprocController>();
            postproc.mOriginalBloomIntensity = 0.5f;
            postproc.peakSettings = postproc.gameSettings;
            postproc.streakSettings = postproc.gameSettings;
            postproc.failSettings = postproc.gameSettings;
        }
        static List<string> defaultEnvironments = new List<string>
        {
            "environment1",
            "environemnt2",
            "environment3",
            "environemnt4",
            "environemnt5",
        };
    }
}