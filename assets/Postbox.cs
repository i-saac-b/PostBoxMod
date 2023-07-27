using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Objects;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.Buildings;
using StardewValley.Menus;
using StardewValley;

namespace PostBoxMod
{
    [XmlType("Mods_ib_Postbox")]
    public class Postbox : Building
    {
        private static bool inUse = false;

        public static string target = "";

        public static List<Tuple<Item, String, Farmer>> outgoing = new List<Tuple<Item, string, Farmer>>();

        public static Item lastDelivery;

        private Rectangle baseSourceRect = new Rectangle(0, 0, 48, 48);

        private static readonly BluePrint Blueprint = new BluePrint("Postbox");

        public Postbox(BluePrint b, Vector2 tileLocation) : base(Postbox.Blueprint, tileLocation)
        { 
        }

        public Postbox() : base(Postbox.Blueprint, Vector2.Zero) 
        {
            inUse = false;
            target = "";
            outgoing = new List<Tuple<Item, string, Farmer>>();
        }

        public override Rectangle getSourceRectForMenu()
        {
            return new Rectangle(0, 0, 64, base.texture.Value.Bounds.Height);
        }

        public override void load()
        {
            base.load();
        }

        public override bool doAction(Vector2 tileLocation, Farmer who)
        {
            if ((int)base.daysOfConstructionLeft <= 0)
            {
                if (inUse){
                    Game1.showGlobalMessage("Someone else is sending a package.");
                    return base.doAction(tileLocation, who);
                }
                inUse = true;
                if (tileLocation.X == (float)((int)base.tileX + 1) && tileLocation.Y == (float)((int)base.tileY + 1))
                {
                    // if player not holding something shippable/giftable 
                    if(target == "" || who.ActiveObject == null || !who.ActiveObject.canBeGivenAsGift())
                    {
                        List<String> friends = new List<string>();
                        foreach (string name in who.friendshipData.Keys)
                        {
                            friends.Add(name);
                        }
                        Game1.activeClickableMenu = new PostboxMenu(friends, PostboxMenu.prepGift);
                    }
                    else if (who.ActiveObject.canBeGivenAsGift() && target != "")
                    {
                        ItemGrabMenu itemGrabMenu = new ItemGrabMenu(null, reverseGrab: true, showReceivingMenu: true, null, loadGift, "", null, snapToBottom: true, canBeExitedWithKey: true, playRightClickSound: false, allowRightClick: true, showOrganizeButton: false, 0, null, -1, this);
                        Game1.activeClickableMenu = itemGrabMenu;
                    }
                    
                }
            }
            inUse = false;
            return base.doAction(tileLocation, who);
        }

        public void loadGift(Item item, Farmer who)
        {
            Game1.showGlobalMessage(who.displayName + " would be sending a " + item.Name + " to " + target + " right about now!");
            if (item != null)
            {
                who.removeItemFromInventory(item);
                outgoing.Add(new Tuple<Item, string, Farmer>(item, target, who));
                lastDelivery = item;
            }
        }

        public override void drawInMenu(SpriteBatch b, int x, int y)
        {
            b.Draw(base.texture.Value, new Vector2(x, y), this.getSourceRectForMenu(), base.color, 0f, new Vector2(0f, 0f), 4f, SpriteEffects.None, 0.89f);
        }

        public override void draw(SpriteBatch b)
        {
            if (base.isMoving)
            {
                return;
            }
            if ((int)base.daysOfConstructionLeft > 0)
            {
                this.drawInConstruction(b);
                return;
            }
            this.drawShadow(b);
            b.Draw(base.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2((int)base.tileX * 64, (int)base.tileY * 64 + (int)base.tilesHigh * 64)), this.baseSourceRect, base.color.Value * base.alpha, 0f, new Vector2(0f, base.texture.Value.Bounds.Height), 4f, SpriteEffects.None, (float)(((int)base.tileY + (int)base.tilesHigh - 1) * 64) / 10000f);
/*            if (this.hasLoadedToday)
            {
                b.Draw(base.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2((int)base.tileX * 64 + 52, (int)base.tileY * 64 + (int)base.tilesHigh * 64 + 276)), new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 700 / 100 * 21, 72, 21, 8), base.color.Value * base.alpha, 0f, new Vector2(0f, base.texture.Value.Bounds.Height), 4f, SpriteEffects.None, (float)(((int)base.tileY + (int)base.tilesHigh) * 64) / 10000f);
            }*/
        }
    }
}
