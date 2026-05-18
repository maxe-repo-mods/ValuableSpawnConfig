# ValuableSpawnConfig

Configure valuable (artifact) spawn amounts, rarity weights, and haul quota with multipliers.

R.E.P.O. BepInEx mod. Host-side only.

## Features

- Global spawn count multiplier to scale all valuable spawns uniformly
- Per-tier multipliers (tiny, small, medium, big, wide, tall, very tall) for fine-grained control
- Rare spawn chance weight multiplier to adjust the frequency of higher-tier valuables
- Haul quota multiplier to adjust the objective amount (useful when spawn counts change)

## Installation

Requires [BepInEx 5.x](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/).

Place `ValuableSpawnConfig.dll` in `BepInEx/plugins/`.

Configuration file is generated at `BepInEx/config/maxenterme.ValuableSpawnConfig.cfg` on first launch.

## Configuration

Edit `BepInEx/config/maxenterme.ValuableSpawnConfig.cfg` to customize spawn behavior.

### Global Spawn Limits

| Section | Key | Default | Range | Description |
|---------|-----|---------|-------|-------------|
| General | SpawnMultiplier | 100 | 0-500 | Global multiplier for total valuable spawn count (100 = 100%, 50 = 50%, 200 = 200%) |

### Per-Tier Multipliers

| Section | Key | Default | Range | Description |
|---------|-----|---------|-------|-------------|
| PerTier | TinyMultiplier | 100 | 0-500 | Multiplier for tiny valuable max amount (100 = 100%) |
| PerTier | SmallMultiplier | 100 | 0-500 | Multiplier for small valuable max amount (100 = 100%) |
| PerTier | MediumMultiplier | 100 | 0-500 | Multiplier for medium valuable max amount (100 = 100%) |
| PerTier | BigMultiplier | 100 | 0-500 | Multiplier for big valuable max amount (100 = 100%) |
| PerTier | WideMultiplier | 100 | 0-500 | Multiplier for wide valuable max amount (100 = 100%) |
| PerTier | TallMultiplier | 100 | 0-500 | Multiplier for tall valuable max amount (100 = 100%) |
| PerTier | VeryTallMultiplier | 100 | 0-500 | Multiplier for very tall valuable max amount (100 = 100%) |

### Rarity and Quota

| Section | Key | Default | Range | Description |
|---------|-----|---------|-------|-------------|
| Rarity | RareChanceMultiplier | 100 | 0-500 | Multiplier for big/wide/tall/veryTall chance weights (100 = 100%, higher = more rare spawns appear) |
| Quota | QuotaMultiplier | 100 | 0-500 | Multiplier applied to the haul quota (100 = 100%, 70 = 70%, useful when spawn counts change) |

## Build

```bash
dotnet build -c Release
```

The compiled DLL will be available at:
```
bin/Release/netstandard2.1/ValuableSpawnConfig.dll
```

## License

MIT
