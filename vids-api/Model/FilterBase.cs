using Newtonsoft.Json;

namespace Vids.Model
{
    public class FilterBase
    {
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; } = 0;

        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 0;

        [JsonProperty("sortBy")]
        public virtual string SortBy { get; set; } = string.Empty;

        [JsonProperty("ascSort")]
        public virtual bool AscSort { get; set; } = false;

        //[JsonProperty("ownerId")]
        //public string OwnerId { get; set; } = string.Empty;
    }
}
