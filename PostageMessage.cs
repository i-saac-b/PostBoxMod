﻿using System;
using System.Collections.Generic;
using System.Text;
using StardewModdingAPI;
using StardewValley.Buildings;
using StardewValley;

namespace PostBoxMod
{
    class PostageMessage
    {
        public string itemId { set; get; }
        public string receiver { set; get; }
        public long senderId { set; get; }
        public PostageMessage(string itemId, String receiver, long senderId)
        {
            this.itemId = itemId;
            this.receiver = receiver;
            this.senderId = senderId;
        }
    }
}
