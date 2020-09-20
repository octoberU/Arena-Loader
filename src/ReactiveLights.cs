using System;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

internal static class ReactiveLightUpdater
{
    public static ReactiveLight[] ReactiveLights;
    public static void GetLightsInScene()
    {
        ReactiveLights = new ReactiveLight[0];
        var lights = GameObject.FindObjectsOfType<Light>();
        List<ReactiveLight> tempList = new List<ReactiveLight>();
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].gameObject.name.Contains("ReactiveLight"))
            {
                tempList.Add(new ReactiveLight(lights[i]));
            }
        }
        ReactiveLights = tempList.ToArray();
    }
    
    public static void Update(float power)
    {
        if (ReactiveLights.Length > 0)
        {
            for (int i = 0; i < ReactiveLights.Length; i++)
            {
                ReactiveLights[i].light.intensity = Mathf.Lerp(0f, ReactiveLights[i].originalIntensity, power*2);
            }
        }
    }

    [HarmonyPatch(typeof(ShaderGlobals), "SetBandPower", new Type[] { typeof(int), typeof(float) })]
    private static class BandPower
    {
        private static void Postfix(ShaderGlobals __instance, int band, float power)
        {
            if (band == 1) Update(power);
        }
    }

}

public struct ReactiveLight
{
    public Light light;
    public float originalIntensity;

    public ReactiveLight(Light light)
    {
        this.light = light;
        this.originalIntensity = light.intensity;
    }
}