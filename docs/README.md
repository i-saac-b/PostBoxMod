# PostBoxMod
Adds a new building that allows sending gifts to NPCs from your farm! No more chasing your favorite villager halfway across the map, or realizing too late that you forgot their schedule.

## Features
New constructable building in Robin's store: the Postbox! Accepts gifts and sends them to NPCs overnight. 
Provides a slight debuff to relationship points gained from gifts given this way (configurable; default 25% reduction)
Compatible with custom NPCs and items!

## Configurability
Configurable relationship point penalty for mailed gifts (Default: .75)

Configurable G cost (Default: 2500)

Configurable Material cost:

- Free: No materials!
- Normal (Default): 2 Iron Bars | 5 Clay | 50 Stone
- Expensive: 10 Iron Bars | 5 Gold Bars | 1 Iridium Bar
- Endgame: 10 Iridium Bars | 5 Radioactive Bars | 10 Battery Packs | 1 Prismatic Shard
- Custom: Loads from the CustomPostboxMaterialCost string. Formatted as one string, "itemId itemCount itemId itemCount [etc.]" (Check out https://stardewlist.com/ for easy item ID lookups!)
