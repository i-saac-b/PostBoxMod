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
        private Building PostboxBuilding;
        private static IModHelper Helper;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Helper = helper;
            Helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            Helper.Events.Content.AssetRequested += this.OnAssetRequested;
            Helper.Events.Display.MenuChanged += OnMenuChanged;
            Helper.Events.GameLoop.DayStarted += this.OnDayStarted;

            this.PostboxTexture = Helper.ModContent.Load<Texture2D>("assets/Postbox.png");
        }

        /*********
        ** Private methods
        *********/
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.Name.IsEquivalentTo("Data/Blueprints"))
            {
                e.Edit(this.EditBluePrints);
            }
            else if (e.Name.IsEquivalentTo("Buildings/Postbox"))
            {
                e.LoadFrom(() => this.PostboxTexture, AssetLoadPriority.Exclusive);
            }
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            object spaceCore = Helper.ModRegistry.GetApi("spacechase0.SpaceCore");
            Helper.Reflection.GetMethod(spaceCore, "RegisterSerializerType").Invoke(typeof(Postbox));
        }

        // add data to the blueprints xnb
        private void EditBluePrints(IAssetData asset)
        {
            asset.AsDictionary<string, string>().Data.Add("Postbox", "388 1/3/2/-1/-1/-2/-1/null/Postbox/For shipping gifts from your farm./Buildings/none/96/96/-1/null/Farm/10/false");
        }

        //debugging
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            Game1.playSound("shwip");
            Monitor.Log("Postboxing initiated", LogLevel.Debug);
            Farm farm = Game1.getFarm();
            for (int i = 0; i < farm.buildings.Count; ++i)  
            {
                Building building = farm.buildings[i];
                Monitor.Log("Detected " + building.ToString(), LogLevel.Debug);
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
                }
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is StardewValley.Menus.CarpenterMenu Menu)
            {
                if (!Helper.Reflection.GetField<bool>(Menu, "magicalConstruction").GetValue())
                {
                    var Blueprints = Helper.Reflection.GetField<List<BluePrint>>(Menu, "blueprints").GetValue();
                    Blueprints.Add(new BluePrint("Postbox"));
                }
            }
        }
    }
}