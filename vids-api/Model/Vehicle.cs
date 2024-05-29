using Vids.DbContexts.Postgres.VidsDb;
using Newtonsoft.Json;

namespace Vids.Model
{
    public class Vehicle
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("lane_id")]
        public string LaneId { get; set; } = string.Empty;

        [JsonProperty("passing_time")]
        public DateTime? PassingTime { get; set; } = null;

        [JsonProperty("speed")]
        public int? Speed { get; set; } = 0;

        [JsonProperty("_class")]
        public string Class { get; set; } = string.Empty;

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;

        public Vehicle() { }

        public Vehicle(vehicle t)
        {
            Id = t.id;
            DeviceId = t.device_id;
            LaneId = t.lane_id;
            PassingTime = t.passing_time;
            Speed = t.speed;
            Class = t._class;
            OwnerId = t.owner_id;
        }
    }

    public class VehicleFilter : FilterBase
    {
        [JsonProperty("sortBy")]
        public override string SortBy { get; set; } = "time";

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("lane_id")]
        public string LaneId { get; set; } = string.Empty;

        [JsonProperty("passing_time")]
        public DateTime? PassingTime { get; set; } = null;

        [JsonProperty("speed")]
        public int? Speed { get; set; } = 0;

        [JsonProperty("_class")]
        public string Class { get; set; } = string.Empty;

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;
    }
}
