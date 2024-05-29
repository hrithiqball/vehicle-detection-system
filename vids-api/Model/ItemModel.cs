using Newtonsoft.Json;

namespace Vids.Model
{
    public class ItemInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
    }
}
