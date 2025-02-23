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