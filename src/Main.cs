using ArenaLoader;
using MelonLoader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

[assembly: AssemblyVersion(ArenaLoaderMod.VERSION)]
[assembly: AssemblyFileVersion(ArenaLoaderMod.VERSION)]
[assembly: MelonGame("Harmonix Music Systems, Inc.", "Audica")]
[assembly: MelonInfo(typeof(ArenaLoaderMod), "Arena Loader", ArenaLoaderMod.VERSION, "octo", "https://github.com/octoberU/Arena-Loader")]

namespace ArenaLoader
{
    public class ArenaLoaderMod : MelonMod
    {
        public const string VERSION = "0.2.4";
        public static string ArenaDirectory => Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Mods", "Arenas");
        public static string SkyboxDirectory => Path.Combine(ArenaDirectory, "Skyboxes");

        public static SkyboxLoader skyboxLoader = new SkyboxLoader();

        /// <summary>
        /// Returns a list of .arena file paths.
        /// </summary>
        public string[] ArenaFiles
        {
            get
            {
                string[] files = Directory.GetFiles(ArenaDirectory);
                return files.Where(x => x.Contains(".arena")).ToArray();
            }
        }

        public static void UpdateSkybox(Material material)
        {
            SkyboxLoader.SetSkybox(material);
            if (SceneReflection == null)
            {
                var reflectionObject = new GameObject();
                var sceneReflection = reflectionObject.AddComponent<ReflectionProbe>();
            }
            SceneReflection.size = new Vector3(5000f, 5000f, 5000f);
            SceneReflection.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;
            SceneReflection.farClipPlane = 0.02f;
            SceneReflection.nearClipPlane = 0.01f;
            SceneReflection.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
            SceneReflection.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
            SceneReflection.RenderProbe();
        }

        public static float CurrentSkyboxExposure
        {
            get => currentSkyboxExposure;
            set
            {
                if (!(value > 1.0f || value < 0.0f)) currentSkyboxExposure = value;
            }
        }



        public static List<string> arenaNames = new List<string>();

        public static float currentSkyboxRotation = 0f;
        private static float currentSkyboxExposure = 1f;
        private static float currentSkyboxReflection = 1f;

        private static ReflectionProbe sceneReflection;
        public static ReflectionProbe SceneReflection
        {
            get
            {
                if (sceneReflection != null) return sceneReflection;
                else
                {
                    sceneReflection = GameObject.FindObjectOfType<ReflectionProbe>(); // Only get an instance of probes if they're null
                    return sceneReflection;
                }
            }
        }

        public static float CurrentSkyboxReflection
        {
            get => currentSkyboxReflection;
            set
            {
                if (!(value > 1.0f || value < 0.0f)) currentSkyboxReflection = value;
            }
        }

        public override void OnApplicationStart()
        {
            Config.RegisterConfig();
            CreateArenaFolders();
            LoadAllFoundArenas();
            PlayerPrefs.SetString("environment_name", "environment1");
        }

        public override void OnModSettingsApplied()
        {
            Config.OnModSettingsApplied();
            string currentArena = PlayerPrefs.GetString("environment_name");
            if (!defaultEnvironments.Contains(currentArena)) TweakBloom(Config.bloomAmount);
        }

        public override void OnApplicationQuit()
        {
            string currentArena = PlayerPrefs.GetString("environment_name");
            Config.lastArena = currentArena;
            if (!defaultEnvironments.Contains(currentArena)) PlayerPrefs.SetString("environment_name", "environment1");
        }

        public override void OnLevelWasLoaded(int level)
        {
            if (level == -1) // Only initiate shader swap on custom arenas.
            {
                MelonCoroutines.Start(ShaderSwap.StartSwap());
                TweakBloom(Config.bloomAmount);
                CurrentSkyboxExposure = 1f;
                CurrentSkyboxReflection = 1f;
                currentSkyboxRotation = 0f;
            }
        }


        private void CreateArenaFolders()
        {
            if (!Directory.Exists(ArenaDirectory))
            {
                Directory.CreateDirectory(ArenaDirectory);
            }
            if (!Directory.Exists(SkyboxDirectory))
            {
                Directory.CreateDirectory(SkyboxDirectory);
            }
        }
        private void LoadAllFoundArenas()
        {
            foreach (string arenaPath in ArenaFiles)
            {
                var bundle = Il2CppAssetBundleManager.LoadFromFile(arenaPath);
                string[] scenePaths = bundle.GetAllScenePaths();
                foreach (var path in scenePaths)
                {
                    string foundScene = System.IO.Path.GetFileNameWithoutExtension(path);
                    arenaNames.Add(foundScene);
                }
            }
        }

        public static bool HasScene(string sceneName) => 
            defaultEnvironments.Contains(sceneName) || arenaNames.Contains(sceneName);

        public static string[] defaultEnvironments =
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
            CurrentSkyboxExposure += amount;
            RenderSettings.skybox.SetFloat("_Exposure", CurrentSkyboxExposure);
        }
        public static void ChangeReflectionStrength(float amount)
        {
            CurrentSkyboxReflection += amount;
            RenderSettings.reflectionIntensity = CurrentSkyboxReflection;
            if (SceneReflection != null) SceneReflection.intensity = CurrentSkyboxReflection;
        }
        public static void TweakBloom(float bloomAmount = 0.5f)
        {
            PostprocController postproc = GameObject.FindObjectOfType<PostprocController>();
            postproc.mOriginalBloomIntensity = bloomAmount;
        }
    }
}






