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
            public const string Version = "0.1.0"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none
        }

        public List<string> arenaFiles;
        public static string[] arenaNames;

        public override void OnApplicationStart()
        {
            var i = HarmonyInstance.Create("ArenaLoader");
            Hooks.ApplyHooks(i);
            CheckArenaFolder();
            arenaFiles = FindArenas();
            LoadAllFoundArenas();
            GetArenaNamesFromFile();
            PlayerPrefs.SetString("environment_name", "environment1");
        }

        private void CheckArenaFolder()
        {
            if(!Directory.Exists(Application.dataPath + "/../Mods/Arenas/"))
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

        public override void OnApplicationQuit()
        {
            PlayerPrefs.SetString("environment_name", "environment1");
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
    }
}






