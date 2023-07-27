using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using StardewValley.Menus;
using StardewValley;

namespace PostBoxMod
{
    class PostboxMenu : ChooseFromListMenu
    {

        public static ClickableTextureComponent lastDeliveryHolder;
        public PostboxMenu(List<string> options, actionOnChoosingListOption chooseAction) : base(options, chooseAction)
        {
            lastDeliveryHolder = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width / 2 - 48, this.yPositionOnScreen + this.height / 2 - 80 - 64, 96, 96), "", Game1.content.LoadString("Strings\\UI:ShippingBin_LastItem"), Game1.mouseCursors, new Rectangle(293, 360, 24, 24), 4f)
            {
                myID = 12598,
                region = 12598
            };
        }

        public static void prepGift(string name)
        {
            Game1.showGlobalMessage("Shipping to " + name);
            Postbox.target = name;
        }
    }
}
