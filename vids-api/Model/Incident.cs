using Newtonsoft.Json;
using Vids.DbContexts.Postgres.VidsDb;

namespace Vids.Model
{
    public class Incident
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("lane_id")]
        public string LaneId { get; set; } = string.Empty;

        [JsonProperty("incident_type")]
        public string IncidentType { get; set; } = string.Empty;

        [JsonProperty("start_time")]
        public DateTime? StartTime { get; set; } = null;

        [JsonProperty("end_time")]
        public DateTime? EndTime { get; set; } = null;

        [JsonProperty("footage")]
        public bool? Footage { get; set; } = false;

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;

        public Incident() { }

        public Incident(incident t)
        {
            Id = t.id;
            DeviceId = t.device_id;
            LaneId = t.lane_id;
            IncidentType = t.incident_type;
            StartTime = t.start_time;
            EndTime = t.end_time;
            Footage = t.footage;
            OwnerId = t.owner_id;
        }
    }

    public class IncidentFilter : FilterBase
    {
        [JsonProperty("sortBy")]
        public override string SortBy { get; set; } = "time";

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("lane_id")]
        public string LaneId { get; set; } = string.Empty;

        [JsonProperty("incident_type")]
        public string IncidentType { get; set; } = string.Empty;

        [JsonProperty("start_time")]
        public DateTime? StartTime { get; set; } = null;

        [JsonProperty("end_time")]
        public DateTime? EndTime { get; set; } = null;

        [JsonProperty("footage")]
        public bool? Footage { get; set; } = false;

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;
    }
}
