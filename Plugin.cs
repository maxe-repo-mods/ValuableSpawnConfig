using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace ValuableSpawnConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "maxenterme.ValuableSpawnConfig";
    private const string PluginName = "ValuableSpawnConfig";
    private const string PluginVersion = "1.1.0";

    internal static Plugin Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;

    // Global multiplier
    internal static ConfigEntry<int> SpawnMultiplier = null!;

    // Per-tier multipliers
    internal static ConfigEntry<int> TinyMultiplier = null!;
    internal static ConfigEntry<int> SmallMultiplier = null!;
    internal static ConfigEntry<int> MediumMultiplier = null!;
    internal static ConfigEntry<int> BigMultiplier = null!;
    internal static ConfigEntry<int> WideMultiplier = null!;
    internal static ConfigEntry<int> TallMultiplier = null!;
    internal static ConfigEntry<int> VeryTallMultiplier = null!;

    // Chance weights
    internal static ConfigEntry<int> RareChanceMultiplier = null!;

    // Quota
    internal static ConfigEntry<int> QuotaMultiplier = null!;

    private void Awake()
    {
        Instance = this;

        SpawnMultiplier = Config.Bind("General", "SpawnMultiplier", 100,
            new ConfigDescription("Global multiplier for total valuable spawn count (100 = 100%)", new AcceptableValueRange<int>(0, 500)));

        TinyMultiplier = Config.Bind("PerTier", "TinyMultiplier", 100,
            new ConfigDescription("Multiplier for tiny valuable max amount (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        SmallMultiplier = Config.Bind("PerTier", "SmallMultiplier", 100,
            new ConfigDescription("Multiplier for small valuable max amount (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        MediumMultiplier = Config.Bind("PerTier", "MediumMultiplier", 100,
            new ConfigDescription("Multiplier for medium valuable max amount (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        BigMultiplier = Config.Bind("PerTier", "BigMultiplier", 100,
            new ConfigDescription("Multiplier for big valuable max amount (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        WideMultiplier = Config.Bind("PerTier", "WideMultiplier", 100,
            new ConfigDescription("Multiplier for wide valuable max amount (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        TallMultiplier = Config.Bind("PerTier", "TallMultiplier", 100,
            new ConfigDescription("Multiplier for tall valuable max amount (100 = 100%)", new AcceptableValueRange<int>(0, 500)));
        VeryTallMultiplier = Config.Bind("PerTier", "VeryTallMultiplier", 100,
            new ConfigDescription("Multiplier for very tall valuable max amount (100 = 100%)", new AcceptableValueRange<int>(0, 500)));

        RareChanceMultiplier = Config.Bind("Rarity", "RareChanceMultiplier", 100,
            new ConfigDescription("Multiplier for big/wide/tall/veryTall chance weights (100 = 100%, higher = more rare spawns)", new AcceptableValueRange<int>(0, 500)));

        QuotaMultiplier = Config.Bind("Quota", "QuotaMultiplier", 100,
            new ConfigDescription(
                "Multiplier applied to the haul quota (100 = 100%, 70 = 70%)",
                new AcceptableValueRange<int>(0, 500)));

        new Harmony(PluginGuid).PatchAll(typeof(Plugin).Assembly);
        Logger.LogInfo($"{PluginName} v{PluginVersion} loaded!");
    }
}
