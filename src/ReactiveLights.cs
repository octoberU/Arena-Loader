using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

internal static class ReactiveLightUpdater
{
    public static List<ReactiveLight> ReactiveLights = new List<ReactiveLight>();
    public static void GetLightsInScene()
    {
        ReactiveLights = new List<ReactiveLight>();
        var lights = GameObject.FindObjectsOfType<Light>();
        
        var tempList = new List<ReactiveLight>();
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].gameObject.name.Contains("ReactiveLight"))
            {
                tempList.Add(new ReactiveLight(lights[i]));
            }
        }
        ReactiveLights = tempList;
    }
    
    public static void Update(float power)
    {
        for (int i = 0; i < ReactiveLights.Count; i++)
            ReactiveLights[i].light.intensity = 
                Mathf.Lerp(0f, ReactiveLights[i].originalIntensity, power * 2);
    }

    [HarmonyPatch(typeof(ShaderGlobals), "SetBandPower", new Type[] { typeof(int), typeof(float) })]
    private static class BandPower
    {
        private static void Postfix(ShaderGlobals __instance, int band, float power)
        {
            if (ReactiveLights.Count == 0) return;
            else if (band == 1) Update(power);
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