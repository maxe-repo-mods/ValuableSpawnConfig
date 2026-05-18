using HarmonyLib;

namespace ValuableSpawnConfig.Patches;

[HarmonyPatch(typeof(RoundDirector))]
public static class RoundDirectorPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("StartRoundLogic")]
    static void StartRoundLogic_Postfix(RoundDirector __instance)
    {
        float mult = Plugin.QuotaMultiplier.Value / 100f;
        if (Plugin.QuotaMultiplier.Value == 100) return;

        int original = __instance.haulGoal;
        __instance.haulGoal = (int)(original * mult);

        Plugin.Logger.LogInfo($"ValuableSpawnConfig: Quota adjusted {original} -> {__instance.haulGoal} (x{mult:F1})");
    }
}
