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
                    materials[j].shader = Shader.Find(materials[j].shader.name.Replace("_swap", ""));
                }

            }
        }
        //RenderSettings.skybox.shader = Shader.Find("Kata/Skybox/6 Sided");
    }
    public static IEnumerator StartSwap()
    {
        yield return new WaitForSeconds(0.001f);
        SwapShaders();
        ReactiveLightUpdater.GetLightsInScene();
    }
}