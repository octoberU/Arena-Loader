using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArenaLoader
{
    public class SkyboxLoader
    {
        public List<Material> skyboxes;

        public Material originalSkybox;

        private int index = 0;

        public int Index
        {
            get
            {
                return index;

            }
            set
            {
                if (value >= (skyboxes.Count)) index = 0;
                else if (value < 0) index = 0;
                else index = value;
            }
        }

        public SkyboxLoader()
        {
            LoadSkyboxes();
        }

        public void LoadSkyboxes()
        {
            if(skyboxes != null)
            {
                foreach (Material skybox in skyboxes)
                {
                    skybox.hideFlags = HideFlags.None;
                    GameObject.Destroy(skybox);

                }
            }
            skyboxes = new List<Material>();
            Shader skyboxShader = Shader.Find("Kata/Skybox/6 Sided");
            string directoryPath = ArenaLoaderMod.SkyboxDirectory;
            if (Directory.Exists(directoryPath))
            {
                foreach (string skyboxDirectory in Directory.GetDirectories(directoryPath))
                {
                    var customSkybox = new Material(skyboxShader);
                    //GameObject.DontDestroyOnLoad(customSkybox);
                    customSkybox.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                    foreach (string filePath in Directory.GetFiles(skyboxDirectory))
                    {
                        if (filePath.Contains("_Top")) customSkybox.SetTexture("_UpTex", LoadTextureFromPath(filePath));
                        else if (filePath.Contains("_Bottom")) customSkybox.SetTexture("_DownTex", LoadTextureFromPath(filePath));
                        else if (filePath.Contains("_Back")) customSkybox.SetTexture("_BackTex", LoadTextureFromPath(filePath));
                        else if (filePath.Contains("_Front")) customSkybox.SetTexture("_FrontTex", LoadTextureFromPath(filePath));
                        else if (filePath.Contains("_Left")) customSkybox.SetTexture("_RightTex", LoadTextureFromPath(filePath));
                        else if (filePath.Contains("_Right")) customSkybox.SetTexture("_LeftTex", LoadTextureFromPath(filePath));
                    }
                    customSkybox.name = skyboxDirectory.Split(Path.DirectorySeparatorChar).LastOrDefault();
                    skyboxes.Add(customSkybox);
                }
            }
        }

        Texture2D LoadTextureFromPath(string path)
        {
            var texture = new Texture2D(2, 2);
            Il2CppImageConversionManager.LoadImage(texture, File.ReadAllBytes(path));
            return texture;
        }

        public static void SetSkybox(Material material)
        {
            RenderSettings.skybox.SetTexture("_UpTex", material.GetTexture("_UpTex"));
            RenderSettings.skybox.SetTexture("_DownTex", material.GetTexture("_DownTex"));
            RenderSettings.skybox.SetTexture("_BackTex", material.GetTexture("_BackTex"));
            RenderSettings.skybox.SetTexture("_FrontTex", material.GetTexture("_FrontTex"));
            RenderSettings.skybox.SetTexture("_RightTex", material.GetTexture("_RightTex"));
            RenderSettings.skybox.SetTexture("_LeftTex", material.GetTexture("_LeftTex"));
        }

    }
}
