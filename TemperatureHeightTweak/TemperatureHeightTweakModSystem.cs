using System;
using HarmonyLib;
using Vintagestory.API.Common;

namespace TemperatureHeightTweak;

public class TemperatureHeightTweakModSystem : ModSystem
{
    public static ICoreAPI Api;
    private Harmony _harmony;

    public override bool ShouldLoad(EnumAppSide side)
    {
        return true;
    }

    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        Api = api;

        string cfgFilename = "TemperatureHeightTweak.json";
        try
        {
            TemperatureHeightTweakConfig cfg;
            if ((cfg = api.LoadModConfig<TemperatureHeightTweakConfig>(cfgFilename)) == null)
            {
                api.StoreModConfig(TemperatureHeightTweakConfig.Loaded, cfgFilename);
            }
            else
            {
                TemperatureHeightTweakConfig.Loaded = cfg;
            }
        }
        catch
        {
            api.StoreModConfig(TemperatureHeightTweakConfig.Loaded, cfgFilename);
        }

        ClimatePatch.A = TemperatureHeightTweakConfig.Loaded.a;
        ClimatePatch.B = TemperatureHeightTweakConfig.Loaded.b;
        ClimatePatch.C = TemperatureHeightTweakConfig.Loaded.c;
        ClimatePatch.Offset = ClimatePatch.A / (1 + Math.Exp(ClimatePatch.B * ClimatePatch.C));
        
        if (!Harmony.HasAnyPatches(Mod.Info.ModID))
        {
            _harmony = new Harmony(Mod.Info.ModID);
            _harmony.PatchAll();
        }
    }

    public override void Dispose()
    {
        _harmony?.UnpatchAll(Mod.Info.ModID);
    }
}