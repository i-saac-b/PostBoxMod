using StardewModdingAPI;
using StardewValley;

namespace PostBoxMod
{
    public static class GameContentHelperExtensions
    {
        // thanks to @atravita from the SDV Discord for this utility function
        public static bool InvalidateCacheAndLocalized(this IGameContentHelper helper, string assetName) => helper.InvalidateCache(assetName)
            | (helper.CurrentLocaleConstant != LocalizedContentManager.LanguageCode.en && helper.InvalidateCache(assetName + "." + helper.CurrentLocale));

    }
}
