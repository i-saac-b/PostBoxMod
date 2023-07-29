using System;
using System.Collections.Generic;
using System.Text;

namespace PostBoxMod
{
    public sealed class ModConfig
    {
        public float PostedGiftRelationshipModifier { get; set; } = .75f;
        public int PostboxCost { get; set; } = 2500;
        public string PostboxMaterialCost { get; set; } = "Normal"; // "Free", "Expensive", "Endgame", "Custom"
        public string CustomPostboxMaterialCost { get; set; } = "113 1";
    }
}
