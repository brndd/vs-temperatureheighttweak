using System;
using HarmonyLib;
using Vintagestory.API.Common;

namespace TemperatureHeightTweak;

[HarmonyPatch]
public class ClimatePatch
{
    // This is the simplified form of
    // a / (1 + exp(-b(x - c))) - a / (1 + exp(bc))
    // where a = 189, b = 0.03, c = 150
    // The values were adjusted arbitrarily until the curve had a nice shape and intersected the vanilla line at ~160 above sea level
    public static double GetAltitudeFactor(int distToSealevel)
    {
        if (distToSealevel < 0)
        {
            return distToSealevel / 1.5;
        }
        else
        {
            return 188.0 / (1.0 + Math.Exp(-0.03 * (distToSealevel - 155.0))) - 1.78;
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Climate), nameof(Climate.GetScaledAdjustedTemperature))]
    public static bool GetScaledAdjustedTemperaturePatch(ref int __result, int unscaledTemp, int distToSealevel)
    {
        __result = Math.Clamp((int) (((double) unscaledTemp - GetAltitudeFactor(distToSealevel)) / (double) Climate.TemperatureScaleConversion) - 20, -20, 40);
        return false; //Skip the original
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Climate), nameof(Climate.GetScaledAdjustedTemperatureFloat))]
    public static bool GetScaledAdjustedTemperatureFloatPatch(ref float __result, int unscaledTemp, int distToSealevel)
    {
        __result = Math.Clamp((float) (((double) unscaledTemp - GetAltitudeFactor(distToSealevel)) / (double) Climate.TemperatureScaleConversion - 20.0), -20f, 40f);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Climate), nameof(Climate.GetScaledAdjustedTemperatureFloatClient))]
    public static bool GetScaledAdjustedTemperatureFloatClientPatch(ref float __result, int unscaledTemp, int distToSealevel)
    {
        __result = Math.Clamp((float) (((double) unscaledTemp - GetAltitudeFactor(distToSealevel)) / (double) Climate.TemperatureScaleConversion - 20.0), -50f, 40f);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Climate), nameof(Climate.GetAdjustedTemperature))]
    public static bool GetAdjustedTemperaturePatch(ref int __result, int unscaledTemp, int distToSealevel)
    {
        __result = (int) Math.Clamp((float) unscaledTemp - GetAltitudeFactor(distToSealevel), 0.0f, (float) byte.MaxValue);
        return false;
    }
}