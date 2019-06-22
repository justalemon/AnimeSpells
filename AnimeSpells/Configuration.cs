using Newtonsoft.Json;

namespace AnimeSpells
{
    public class Configuration
    {
        [JsonProperty("mana_x")]
        public float ManaX { get; set; }
        [JsonProperty("mana_y")]
        public float ManaY { get; set; }
        [JsonProperty("mana_w")]
        public float ManaWidth { get; set; }
        [JsonProperty("mana_h")]
        public float ManaHeight { get; set; }
        [JsonProperty("mana_offset_x")]
        public float ManaOffsetX { get; set; }
        [JsonProperty("mana_offset_y")]
        public float ManaOffsetY { get; set; }
        [JsonProperty("mana_offset_w")]
        public float ManaOffsetWidth { get; set; }
        [JsonProperty("mana_offset_h")]
        public float ManaOffsetHeight { get; set; }
    }
}
