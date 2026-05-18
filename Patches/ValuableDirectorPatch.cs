using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ValuableSpawnConfig.Patches;

[HarmonyPatch]
public static class ValuableDirectorPatch
{
    private static readonly Dictionary<ValuableVolume.Type, int> _tierSpawnCount = new();
    private static readonly Dictionary<ValuableVolume.Type, int> _tierAdjustedMax = new();
    private static bool _initialized;
    private static readonly FieldInfo _totalMaxValueField =
        AccessTools.Field(typeof(ValuableDirector), "totalMaxValue");

    [HarmonyPatch(typeof(ValuableDirector), nameof(ValuableDirector.SetupHost))]
    [HarmonyPrefix]
    private static void SetupHost_Prefix()
    {
        _tierSpawnCount.Clear();
        _tierAdjustedMax.Clear();
        _initialized = false;
    }
    [HarmonyPatch(typeof(ValuableDirector), "SpawnValuable")]
    [HarmonyPrefix]
    private static bool SpawnValuable_Prefix(ValuableVolume _volume, ValuableDirector __instance)
    {
        try
        {
            if (!SemiFunc.IsMasterClientOrSingleplayer()) return true;

            // First call: read original maxAmounts from fields (set by SetupHost curves)
            // and compute adjusted limits
            if (!_initialized)
            {
                _initialized = true;
                InitializeAdjustedLimits(__instance);
            }

            var tier = _volume.VolumeType;

            // Get adjusted max for this tier
            if (!_tierAdjustedMax.TryGetValue(tier, out int adjustedMax))
                return true; // unknown tier, allow

            // Track spawn count
            if (!_tierSpawnCount.TryGetValue(tier, out int count))
                count = 0;

            if (count >= adjustedMax)
                return false;

            _tierSpawnCount[tier] = count + 1;
            return true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"ValuableSpawnConfig Spawn_Prefix error: {e}");
            return true;
        }
    }

    private static void InitializeAdjustedLimits(ValuableDirector instance)
    {
        // Read originals from fields (set by SetupHost curves)
        var tiers = new (ValuableVolume.Type type, int origMax, float mult)[]
        {
            (ValuableVolume.Type.Tiny, instance.tinyMaxAmount, Plugin.TinyMultiplier.Value / 100f),
            (ValuableVolume.Type.Small, instance.smallMaxAmount, Plugin.SmallMultiplier.Value / 100f),
            (ValuableVolume.Type.Medium, instance.mediumMaxAmount, Plugin.MediumMultiplier.Value / 100f),
            (ValuableVolume.Type.Big, instance.bigMaxAmount, Plugin.BigMultiplier.Value / 100f),
            (ValuableVolume.Type.Wide, instance.wideMaxAmount, Plugin.WideMultiplier.Value / 100f),
            (ValuableVolume.Type.Tall, instance.tallMaxAmount, Plugin.TallMultiplier.Value / 100f),
            (ValuableVolume.Type.VeryTall, instance.veryTallMaxAmount, Plugin.VeryTallMultiplier.Value / 100f),
        };

        foreach (var (type, origMax, mult) in tiers)
        {
            _tierAdjustedMax[type] = Mathf.RoundToInt(origMax * mult);
        }

        // Apply global spawn multiplier to totalMaxAmount
        // The spawn loop reads this field directly: for (_i = 0; _i < totalMaxAmount; _i++)
        float spawnMult = Plugin.SpawnMultiplier.Value / 100f;
        if (Plugin.SpawnMultiplier.Value != 100)
        {
            int origTotal = instance.totalMaxAmount;
            instance.totalMaxAmount = Mathf.RoundToInt(origTotal * spawnMult);

            // Also scale value budget
            if (_totalMaxValueField != null)
            {
                float origValue = (float)_totalMaxValueField.GetValue(instance);
                _totalMaxValueField.SetValue(instance, origValue * spawnMult);
            }
        }

        // Apply rare chance multiplier
        float rareMult = Plugin.RareChanceMultiplier.Value / 100f;
        if (Plugin.RareChanceMultiplier.Value != 100)
        {
            instance.bigChance = Mathf.RoundToInt(instance.bigChance * rareMult);
            instance.wideChance = Mathf.RoundToInt(instance.wideChance * rareMult);
            instance.tallChance = Mathf.RoundToInt(instance.tallChance * rareMult);
            instance.veryTallChance = Mathf.RoundToInt(instance.veryTallChance * rareMult);
        }

        Plugin.Logger.LogDebug($"ValuableSpawnConfig: total={instance.totalMaxAmount}, " +
            $"tiny={_tierAdjustedMax[ValuableVolume.Type.Tiny]}, small={_tierAdjustedMax[ValuableVolume.Type.Small]}, " +
            $"medium={_tierAdjustedMax[ValuableVolume.Type.Medium]}, big={_tierAdjustedMax[ValuableVolume.Type.Big]}, " +
            $"wide={_tierAdjustedMax[ValuableVolume.Type.Wide]}, tall={_tierAdjustedMax[ValuableVolume.Type.Tall]}, " +
            $"veryTall={_tierAdjustedMax[ValuableVolume.Type.VeryTall]}");
    }
}
