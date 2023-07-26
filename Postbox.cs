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
using StardewValley;

namespace PostBoxMod
{

    public class Postbox : Building
    {
        [XmlElement("input")]
        public readonly NetRef<Chest> input = new NetRef<Chest>();

        private bool hasLoadedToday;

        private Rectangle baseSourceRect = new Rectangle(0, 0, 64, 128);

        private static readonly BluePrint Blueprint = new BluePrint("Postbox");

        public Postbox(BluePrint b, Vector2 tileLocation) : base(Postbox.Blueprint, tileLocation)
        {

            this.input.Value = new Chest(playerChest: true);
        }

        public Postbox() : base(Postbox.Blueprint, Vector2.Zero)
        {
        }

        protected override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddFields(this.input);
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
            Game1.showGlobalMessage("doAction");
            Game1.playSound("shwip");
            if ((int)base.daysOfConstructionLeft <= 0)
            {
                if (tileLocation.X == (float)((int)base.tileX + 1) && tileLocation.Y == (float)((int)base.tileY + 1))
                {
                    Game1.showGlobalMessage("Engaging postbox");
                }
            }
            return base.doAction(tileLocation, who);
        }

        public override void dayUpdate(int dayOfMonth)
        {
            this.hasLoadedToday = false;
            for (int i = this.input.Value.items.Count - 1; i >= 0; i--)
            {
                if (this.input.Value.items[i] != null)
                {

                }
            }
            base.dayUpdate(dayOfMonth);
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
            if (this.hasLoadedToday)
            {
                b.Draw(base.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2((int)base.tileX * 64 + 52, (int)base.tileY * 64 + (int)base.tilesHigh * 64 + 276)), new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 700 / 100 * 21, 72, 21, 8), base.color.Value * base.alpha, 0f, new Vector2(0f, base.texture.Value.Bounds.Height), 4f, SpriteEffects.None, (float)(((int)base.tileY + (int)base.tilesHigh) * 64) / 10000f);
            }
        }
    }
}
