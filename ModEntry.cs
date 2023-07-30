using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;

namespace PostBoxMod
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {

        /*********
        ** Fields
        **********/

        private Texture2D PostboxTexture;
        private ModConfig Config;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // Read in config file and create if needed
            try
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
            }
            catch (Exception)
            {
                this.Config = new ModConfig();
                this.Monitor.Log(this.Helper.Translation.Get("entry.badConfig"), LogLevel.Warn);
            }

            Postbox.Initialize(this.Monitor, this.Config, Helper);
            PostageMenu.Initialize(this.Monitor, Helper);

            Helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            Helper.Events.Content.AssetRequested += this.OnAssetRequested;
            Helper.Events.Display.MenuChanged += this.OnMenuChanged;
            Helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;

            this.PostboxTexture = Helper.ModContent.Load<Texture2D>("assets/Postbox.png");
        }

        /*********
        ** Private methods
        *********/
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Blueprints"))
            {
                Monitor.Log("Editing Blueprints", LogLevel.Debug);
                e.Edit(this.EditBluePrints);
            }
            else if (e.Name.IsEquivalentTo("Buildings/Postbox"))
            {
                Monitor.Log("Loading Postbox", LogLevel.Debug);
                e.LoadFrom(() => this.PostboxTexture, AssetLoadPriority.Exclusive);
            }
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            object spaceCore = Helper.ModRegistry.GetApi("spacechase0.SpaceCore");
            Helper.Reflection.GetMethod(spaceCore, "RegisterSerializerType").Invoke(typeof(Postbox));
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.Helper.GameContent.InvalidateCacheAndLocalized("Data/Blueprints");
        }

        // add data to the blueprints xnb
        private void EditBluePrints(IAssetData asset)
        {
            int cost = Config.PostboxCost;
            string material = "335 3 330 5 390 50";
            switch (Config.PostboxMaterialCost) {
                case "Normal": break;
                case "Free": material = ""; break;
                case "Expensive": material = "335 10 336 5 337 1"; break;
                case "Endgame": material = "337 10 910 5 787 10 74 1"; break;
                case "Custom": material = Config.CustomPostboxMaterialCost; break;
                default: break;
            }
            asset.AsDictionary<string, string>().Data.Add("Postbox", $"{material}/3/2/-1/-1/-2/-1/null/{Helper.Translation.Get("blueprint-title")}/{Helper.Translation.Get("blueprint-description")}/Buildings/none/96/96/-1/null/Farm/{cost}/false");
        }

        //debugging
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            Monitor.Log("Checking for unconverted Postbox...", LogLevel.Debug);
            Farm farm = Game1.getFarm();
            for (int i = 0; i < farm.buildings.Count; ++i)  
            {
                Building building = farm.buildings[i];
                Monitor.Log("Detected " + building.ToString() + " with " + building.buildingType.Value, LogLevel.Debug);
                if (building.buildingType.Value == "Postbox" && !(building is Postbox))
                {
                    farm.buildings[i] = new Postbox();
                    farm.buildings[i].buildingType.Value = building.buildingType.Value;
                    farm.buildings[i].daysOfConstructionLeft.Value = building.daysOfConstructionLeft.Value;
                    farm.buildings[i].indoors.Value = building.indoors.Value;
                    farm.buildings[i].tileX.Value = building.tileX.Value;
                    farm.buildings[i].tileY.Value = building.tileY.Value;
                    farm.buildings[i].tilesWide.Value = building.tilesWide.Value;
                    farm.buildings[i].tilesHigh.Value = building.tilesHigh.Value;
                    farm.buildings[i].load();
                    Monitor.Log("Converted.", LogLevel.Debug);
                }
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is StardewValley.Menus.CarpenterMenu Menu)
            {
                Monitor.Log("In Carpenter", LogLevel.Debug);
                Monitor.Log("Check result " + Helper.Reflection.GetField<bool>(Menu, "magicalConstruction").GetValue(), LogLevel.Debug);
                if (!Helper.Reflection.GetField<bool>(Menu, "magicalConstruction").GetValue())
                {
                    Monitor.Log("Adding Postbox", LogLevel.Debug);
                    var Blueprints = Helper.Reflection.GetField<List<BluePrint>>(Menu, "blueprints").GetValue();
                    Blueprints.Add(new BluePrint("Postbox"));
                }
            }
        }
    }
}