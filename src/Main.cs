using MelonLoader;
using UnityEngine;
using Harmony;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AudicaModding
{
    public class AudicaMod : MelonMod
    {
        public static class BuildInfo
        {
            public const string Name = "ArenaLoader";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "octo"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "0.1.3"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none
        }

        public List<string> arenaFiles;
        public static string[] arenaNames;
        public static float currentSkyboxRotation;
        public static float currentSkyboxExposure = 1f;
        public static float currentSkyboxReflection = 1f;

        public override void OnApplicationStart()
        {
            var i = HarmonyInstance.Create("ArenaLoader");
            Hooks.ApplyHooks(i);
            CheckArenaFolder();
            arenaFiles = FindArenas();
            LoadAllFoundArenas();
            GetArenaNamesFromFile();
            //CheckConfig();
            PlayerPrefs.SetString("environment_name", "environment1");
        }

        public override void OnApplicationQuit()
        {
            ModPrefs.SetString("ArenaLoader", "LastArena", PlayerPrefs.GetString("environment_name"));
            PlayerPrefs.SetString("environment_name", "environment1");
        }


        private void CheckConfig()
        {
            if (!ModPrefs.HasKey("ArenaLoader", "LastArena"))
            {
                ModPrefs.RegisterPrefString("ArenaLoader", "LastArena", PlayerPrefs.GetString("environment_name"));
            }
            else
            {
                PlayerPrefs.SetString("environment_name", "environment1");
            }
        }

        private void LoadLastArena()
        {
            string currentArena = ModPrefs.GetString("ArenaLoader", "LastArena");
            if (ArenaExists(currentArena))
            {
                PlayerPrefs.SetString("environment_name", currentArena);
            }
            else
            {
                PlayerPrefs.SetString("environment_name", "environment1");
            }
        }

        private bool ArenaExists(string currentArena)
        {
            bool inDefaultArenas = defaultEnvironments.Contains(currentArena);
            bool inCustomArenas = arenaNames.Contains(currentArena);
            return inDefaultArenas || inCustomArenas;
        }

        private void CheckArenaFolder()
        {
            if (!Directory.Exists(Application.dataPath + "/../Mods/Arenas/"))
            {
                Directory.CreateDirectory(Application.dataPath + "/../Mods/Arenas/");
            }
        }
        private void LoadAllFoundArenas()
        {
            foreach (string arenaPath in arenaFiles)
            {
                Il2CppAssetBundleManager.LoadFromFile(arenaPath);
            }
        }

        private void GetArenaNamesFromFile()
        {
            arenaNames = new string[arenaFiles.Count];
            for (int i = 0; i < arenaFiles.Count; i++)
            {
                string arena = arenaFiles[i].Split('/').Last().Replace(".arena", "");
                arenaNames[i] = arena;
            }
        }



        private List<string> FindArenas()
        {
            string[] files = Directory.GetFiles(Application.dataPath + "/../Mods/Arenas/");
            List<string> arenas = new List<string>();
            foreach (string file in files)
            {
                if (file.Contains(".arena"))
                {
                    arenas.Add(file);
                    MelonModLogger.Log(file);
                }
            }
            return arenas;
        }

        public static List<string> defaultEnvironments = new List<string>
        {
            "environment1",
            "environment2",
            "environment3",
            "environment4",
            "environment5",
        };

        public static void RotateSkybox(float amount)
        {
            currentSkyboxRotation += amount;
            RenderSettings.skybox.SetFloat("_Rotation", currentSkyboxRotation);
        }
        public static void ChangeExposure(float amount)
        {
            currentSkyboxExposure += amount;
            RenderSettings.skybox.SetFloat("_Exposure", currentSkyboxExposure);
        }
        public static void ChangeReflectionStrength(float amount)
        {
            currentSkyboxReflection += amount;
            RenderSettings.reflectionIntensity = currentSkyboxReflection;
        }

    }
}






