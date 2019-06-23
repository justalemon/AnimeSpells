using Newtonsoft.Json;

namespace AnimeSpells
{
    public class Configuration
    {
        [JsonProperty("mana_max")]
        public int ManaMax { get; set; }
        [JsonProperty("mana_regen")]
        public int ManaRegen { get; set; }

        [JsonProperty("count_x")]
        public float CountX { get; set; }
        [JsonProperty("count_y")]
        public float CountY { get; set; }

        [JsonProperty("bar_x")]
        public float BarX { get; set; }
        [JsonProperty("bar_y")]
        public float BarY { get; set; }
        [JsonProperty("bar_w")]
        public float BarWidth { get; set; }
        [JsonProperty("bar_h")]
        public float BarHeight { get; set; }
        [JsonProperty("bar_offset_x")]
        public float BarOffsetX { get; set; }
        [JsonProperty("bar_offset_y")]
        public float BarOffsetY { get; set; }
        [JsonProperty("bar_offset_w")]
        public float BarOffsetWidth { get; set; }
        [JsonProperty("bar_offset_h")]
        public float BarOffsetHeight { get; set; }
    }
}
