using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal static class ShaderSwap
{
    public static void SwapShaders()
    {
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;
            for (int j = 0; j < materials.Length; j++)
            {
                if (materials[j].shader.name.Contains("_swap"))
                {
                    //MelonModLogger.Log($"Swapping {materials[j].shader.name} to {materials[j].shader.name.Replace("_swap", "")}");
                    materials[j].shader = Shader.Find(materials[j].shader.name.Replace("_swap", ""));
                }

            }
        }
        RenderSettings.skybox.shader = Shader.Find("Kata/Skybox/6 Sided");

    }

    public static void CacheShaders()
    {
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
        MelonModLogger.Log("Swapping Shaders!");
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;
            MelonModLogger.Log("Swapping");
            for (int j = 0; j < materials.Length; j++)
            {
                MelonModLogger.Log(renderers[i].gameObject.name);
                MelonModLogger.Log(materials[j].shader.name);
            }
        }
    }

    public static IEnumerator StartSwap()
    {
        yield return new WaitForSeconds(5f);
        SwapShaders();
        ReactiveLightUpdater.GetLightsInScene();
    }
}