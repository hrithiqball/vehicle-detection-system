using Vids.DbContexts.Postgres.VidsDb;
using Vids.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using NLog.Fluent;

namespace Vids.Model
{
    public class TrafficData
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("lane_id")]
        public string LaneId { get; set; } = string.Empty;

        [JsonProperty("traffic_time")]
        public DateTime? TrafficTime { get; set; } = null;

        [JsonProperty("c1")]
        public int? C1 { get; set; } = 0;

        [JsonProperty("c2")]
        public int? C2 { get; set; } = 0;

        [JsonProperty("c3")]
        public int? C3 { get; set; } = 0;

        [JsonProperty("c4")]
        public int? C4 { get; set; } = 0;

        [JsonProperty("c5")]
        public int? C5 { get; set; } = 0;

        [JsonProperty("c6")]
        public int? C6 { get; set; } = 0;

        [JsonProperty("c7")]
        public int? C7 { get; set; } = 0;

        [JsonProperty("total_vol")]
        public int? TotalVol { get; set; } = 0;

        [JsonProperty("flow_rate")]
        public int? FlowRate { get; set; } = 0;

        [JsonProperty("speed")]
        public int? Speed { get; set; } = 0;

        [JsonProperty("headway")]
        public decimal? Headway { get; set; } = 0;

        [JsonProperty("los")]
        public string Los { get; set; } = string.Empty;

        [JsonProperty("gap")]
        public decimal? Gap { get; set; } = 0;

        [JsonProperty("c1_flow_rate")]
        public int? C1FlowRate { get; set; } = 0;

        [JsonProperty("c2_flow_rate")]
        public int? C2FlowRate { get; set; } = 0;

        [JsonProperty("c3_flow_rate")]
        public int? C3FlowRate { get; set; } = 0;

        [JsonProperty("c4_flow_rate")]
        public int? C4FlowRate { get; set; } = 0;

        [JsonProperty("c5_flow_rate")]
        public int? C5FlowRate { get; set; } = 0;

        [JsonProperty("c6_flow_rate")]
        public int? C6FlowRate { get; set; } = 0;

        [JsonProperty("c7_flow_rate")]
        public int? C7FlowRate { get; set; } = 0;

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;

        public TrafficData() { }

        public TrafficData(traffic_data t)
        {
            Id = t.id;
            DeviceId = t.device_id;
            LaneId = t.lane_id;
            TrafficTime = t.traffic_time;
            C1 = t.c1;
            C2 = t.c2;
            C3 = t.c3;
            C4 = t.c4;
            C5 = t.c5;
            C6 = t.c6;
            C7 = t.c7;
            TotalVol = t.total_vol;
            FlowRate = t.flow_rate;
            Speed = t.speed;
            Headway = t.headway;
            Los = t.los;
            Gap = t.gap;
            C1FlowRate = t.c1_flow_rate;
            C2FlowRate = t.c2_flow_rate;
            C3FlowRate = t.c3_flow_rate;
            C4FlowRate = t.c4_flow_rate;
            C5FlowRate = t.c5_flow_rate;
            C6FlowRate = t.c6_flow_rate;
            C7FlowRate = t.c7_flow_rate;
        }
    }

    public class TrafficDataFilter : FilterBase
    {
        [JsonProperty("sortBy")]
        public override string SortBy { get; set; } = "received_time";

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("lane_id")]
        public string LaneId { get; set; } = string.Empty;

        [JsonProperty("traffic_time")]
        public DateTime? TrafficTime { get; set; } = null;

        [JsonProperty("total_vol")]
        public int? TotalVol { get; set; } = 0;

        [JsonProperty("flow_rate")]
        public int? FlowRate { get; set; } = 0;

        [JsonProperty("speed")]
        public int? Speed { get; set; } = 0;

        [JsonProperty("headway")]
        public decimal? Headway { get; set; } = 0;

        [JsonProperty("los")]
        public string Los { get; set; } = string.Empty;

        [JsonProperty("gap")]
        public decimal? Gap { get; set; } = 0;

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;
    }

    public class BulkDeleteTrafficData
    {
        [JsonProperty("itemList")]
        public List<ItemInfo> ItemList { get; set; } = new List<ItemInfo>();
    }

    public class FlowRate
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("c1_flow_rate")]
        public int? C1FlowRate { get; set; } = 0;

        [JsonProperty("c2_flow_rate")]
        public int? C2FlowRate { get; set; } = 0;

        [JsonProperty("c3_flow_rate")]
        public int? C3FlowRate { get; set; } = 0;

        [JsonProperty("c4_flow_rate")]
        public int? C4FlowRate { get; set; } = 0;

        [JsonProperty("c5_flow_rate")]
        public int? C5FlowRate { get; set; } = 0;

        [JsonProperty("c6_flow_rate")]
        public int? C6FlowRate { get; set; } = 0;

        [JsonProperty("c7_flow_rate")]
        public int? C7FlowRate { get; set; } = 0;

        public FlowRate(traffic_data t)
        {
            Id = t.id;
            C1FlowRate = t.c1_flow_rate;
            C2FlowRate = t.c2_flow_rate;
            C3FlowRate = t.c3_flow_rate;
            C4FlowRate = t.c4_flow_rate;
            C5FlowRate = t.c5_flow_rate;
            C6FlowRate = t.c6_flow_rate;
            C7FlowRate = t.c7_flow_rate;
        }
    }

    public class ClassVolume
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("c1")]
        public int? C1 { get; set; } = 0;

        [JsonProperty("c2")]
        public int? C2 { get; set; } = 0;

        [JsonProperty("c3")]
        public int? C3 { get; set; } = 0;

        [JsonProperty("c4")]
        public int? C4 { get; set; } = 0;

        [JsonProperty("c5")]
        public int? C5 { get; set; } = 0;

        [JsonProperty("c6")]
        public int? C6 { get; set; } = 0;

        [JsonProperty("c7")]
        public int? C7 { get; set; } = 0;

        public ClassVolume(traffic_data t)
        {
            Id = t.id;
            C1 = t.c1;
            C2 = t.c2;
            C3 = t.c3;
            C4 = t.c4;
            C5 = t.c5;
            C6 = t.c6;
            C7 = t.c7;
        }
    }
}
