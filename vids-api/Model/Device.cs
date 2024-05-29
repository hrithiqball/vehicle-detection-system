using Vids.DbContexts.Postgres.VidsDb;
using Vids.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vids.Model
{
    public class Device
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("device_name")]
        public string DeviceName { get; set; } = string.Empty;

        [JsonProperty("device_name_2")]
        public string DeviceName2 { get; set; } = string.Empty;

        //public ClassName LaneName { get; set } = new ClassName(Device);
        [JsonProperty("device_tag")]
        public string DeviceTag { get; set; } = string.Empty;

        [JsonProperty("ip_address")]
        public string IpAddress { get; set; } = string.Empty;

        [JsonProperty("location")]
        public string Location { get; set; } = string.Empty;

        [JsonProperty("bound")]
        public string Bound { get; set; } = string.Empty;

        [JsonProperty("km")]
        public decimal? Km { get; set; } = 0;

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; } = 0;

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; } = 0;

        [JsonProperty("control_room")]
        public string ControlRoom { get; set; } = string.Empty;

        [JsonProperty("total_lane")]
        public int? TotalLane { get; set; } = 0;

        [JsonProperty("lane1_id")]
        public string Lane1Id { get; set; } = string.Empty;

        [JsonProperty("lane1_name")]
        public string Lane1Name { get; set; } = string.Empty;

        [JsonProperty("lane2_id")]
        public string Lane2Id { get; set; } = string.Empty;

        [JsonProperty("lane2_name")]
        public string Lane2Name { get; set; } = string.Empty;

        [JsonProperty("lane3_id")]
        public string Lane3Id { get; set; } = string.Empty;

        [JsonProperty("lane3_name")]
        public string Lane3Name { get; set; } = string.Empty;

        [JsonProperty("lane4_id")]
        public string Lane4Id { get; set; } = string.Empty;

        [JsonProperty("lane4_name")]
        public string Lane4Name { get; set; } = string.Empty;

        [JsonProperty("lane5_id")]
        public string Lane5Id { get; set; } = string.Empty;

        [JsonProperty("lane5_name")]
        public string Lane5Name { get; set; } = string.Empty;

        [JsonProperty("lane6_id")]
        public string Lane6Id { get; set; } = string.Empty;

        [JsonProperty("lane6_name")]
        public string Lane6Name { get; set; } = string.Empty;

        [JsonProperty("congestion_line")]
        public string CongestionLine { get; set; } = string.Empty;

        [JsonProperty("camera_id")]
        public string CameraId { get; set; } = string.Empty;

        [JsonProperty("free_flow_speed")]
        public int? FreeFlowSpeed { get; set; } = 0;

        [JsonProperty("road_capacity")]
        public int? RoadCapacity { get; set; } = 0;

        [JsonProperty("total_class")]
        public int? TotalClass { get; set; } = 0;

        [JsonProperty("c1_name")]
        public string C1Name { get; set; } = string.Empty;

        [JsonProperty("c2_name")]
        public string C2Name { get; set; } = string.Empty;

        [JsonProperty("c3_name")]
        public string C3Name { get; set; } = string.Empty;

        [JsonProperty("c4_name")]
        public string C4Name { get; set; } = string.Empty;

        [JsonProperty("c5_name")]
        public string C5Name { get; set; } = string.Empty;

        [JsonProperty("c6_name")]
        public string C6Name { get; set; } = string.Empty;

        [JsonProperty("c7_name")]
        public string C7Name { get; set; } = string.Empty;

        [JsonProperty("has_speed")]
        public bool? HasSpeed { get; set; } = false;

        [JsonProperty("has_headway")]
        public bool? HasHeadway { get; set; } = false;

        [JsonProperty("has_occupancy")]
        public bool? HasOccupancy { get; set; } = false;

        [JsonProperty("has_gap")]
        public bool? HasGap { get; set; } = false;

        [JsonProperty("has_flow_rate")]
        public bool? HasFlowRate { get; set; } = false;

        [JsonProperty("has_c1_flow_rate")]
        public bool? HasC1FlowRate { get; set; } = false;

        [JsonProperty("has_c2_flow_rate")]
        public bool? HasC2FlowRate { get; set; } = false;

        [JsonProperty("has_c3_flow_rate")]
        public bool? HasC3FlowRate { get; set; } = false;

        [JsonProperty("has_c4_flow_rate")]
        public bool? HasC4FlowRate { get; set; } = false;

        [JsonProperty("has_c5_flow_rate")]
        public bool? HasC5FlowRate { get; set; } = false;

        [JsonProperty("has_c6_flow_rate")]
        public bool? HasC6FlowRate { get; set; } = false;

        [JsonProperty("has_c7_flow_rate")]
        public bool? HasC7FlowRate { get; set; } = false;

        [JsonProperty("has_los")]
        public bool? HasLos { get; set; } = false;

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; } = string.Empty;

        [JsonProperty("created_time")]
        public DateTime? CreatedTime { get; set; } = null;

        public Device() { }

        public Device(device t)
        {
            DeviceId = t.device_id;
            DeviceName = t.device_name;
            DeviceName2 = t.device_name_2;
            DeviceTag = t.device_tag;
            IpAddress = t.ip_address;
            Location = t.location;
            Bound = t.bound;
            Km = t.km;
            Latitude = t.latitude;
            Longitude = t.longitude;
            ControlRoom = t.control_room;
            TotalLane = t.total_lane;
            Lane1Id = t.lane1_id;
            Lane1Name = t.lane1_name;
            Lane2Id = t.lane2_id;
            Lane2Name = t.lane2_name;
            Lane3Id = t.lane3_id;
            Lane3Name = t.lane3_name;
            Lane4Id = t.lane4_id;
            Lane4Name = t.lane4_name;
            Lane5Id = t.lane5_id;
            Lane5Name = t.lane5_name;
            Lane6Id = t.lane6_id;
            Lane6Name = t.lane6_name;
            CongestionLine = t.congestion_line;
            CameraId = t.camera_id;
            FreeFlowSpeed = t.free_flow_speed;
            RoadCapacity = t.road_capacity;
            TotalClass = t.total_class;
            C1Name = t.c1_name;
            C2Name = t.c2_name;
            C3Name = t.c3_name;
            C4Name = t.c4_name;
            C5Name = t.c5_name;
            C6Name = t.c6_name;
            C7Name = t.c7_name;
            HasSpeed = t.has_speed;
            HasHeadway = t.has_headway;
            HasOccupancy = t.has_occupancy;
            HasGap = t.has_gap;
            HasFlowRate = t.has_flow_rate;
            HasC1FlowRate = t.has_c1_flow_rate;
            HasC2FlowRate = t.has_c2_flow_rate;
            HasC3FlowRate = t.has_c3_flow_rate;
            HasC4FlowRate = t.has_c4_flow_rate;
            HasC5FlowRate = t.has_c5_flow_rate;
            HasC6FlowRate = t.has_c6_flow_rate;
            HasC7FlowRate = t.has_c7_flow_rate;
            HasLos = t.has_los;
            CreatedTime = t.created_time;
        }
    }

    public class DeviceFilter : FilterBase
    {
        [JsonProperty("sortBy")]
        public override string SortBy { get; set; } = "time";

        [JsonProperty("control_room")]
        public string ControlRoom { get; set; } = string.Empty;

        [JsonProperty("total_lane")]
        public int? TotalLane { get; set; } = 0;

        [JsonProperty("camera_id")]
        public string CameraId { get; set; } = string.Empty;

        [JsonProperty("created_time")]
        public DateTime? CreatedTime { get; set; } = null;
    }

    public class LaneName
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("lane1_id")]
        public string Lane1Id { get; set; } = string.Empty;

        [JsonProperty("lane1_name")]
        public string Lane1Name { get; set; } = string.Empty;

        [JsonProperty("lane2_id")]
        public string Lane2Id { get; set; } = string.Empty;

        [JsonProperty("lane2_name")]
        public string Lane2Name { get; set; } = string.Empty;

        [JsonProperty("lane3_id")]
        public string Lane3Id { get; set; } = string.Empty;

        [JsonProperty("lane3_name")]
        public string Lane3Name { get; set; } = string.Empty;

        [JsonProperty("lane4_id")]
        public string Lane4Id { get; set; } = string.Empty;

        [JsonProperty("lane4_name")]
        public string Lane4Name { get; set; } = string.Empty;

        [JsonProperty("lane5_id")]
        public string Lane5Id { get; set; } = string.Empty;

        [JsonProperty("lane5_name")]
        public string Lane5Name { get; set; } = string.Empty;

        [JsonProperty("lane6_id")]
        public string Lane6Id { get; set; } = string.Empty;

        [JsonProperty("lane6_name")]
        public string Lane6Name { get; set; } = string.Empty;

        public LaneName(device t)
        {
            DeviceId = t.device_id;
            Lane1Name = t.lane1_name;
            Lane2Name = t.lane2_name;
            Lane3Name = t.lane3_name;
            Lane4Name = t.lane4_name;
            Lane5Name = t.lane5_name;
            Lane6Name = t.lane6_name;
            Lane1Id = t.lane1_id;
            Lane2Id = t.lane2_id;
            Lane3Id = t.lane3_id;
            Lane4Id = t.lane4_id;
            Lane5Id = t.lane5_id;
            Lane6Id = t.lane6_id;
        }
    }

    public class ClassName
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("c1_name")]
        public string C1Name { get; set; } = string.Empty;

        [JsonProperty("c2_name")]
        public string C2Name { get; set; } = string.Empty;

        [JsonProperty("c3_name")]
        public string C3Name { get; set; } = string.Empty;

        [JsonProperty("c4_name")]
        public string C4Name { get; set; } = string.Empty;

        [JsonProperty("c5_name")]
        public string C5Name { get; set; } = string.Empty;

        [JsonProperty("c6_name")]
        public string C6Name { get; set; } = string.Empty;

        [JsonProperty("c7_name")]
        public string C7Name { get; set; } = string.Empty;

        public ClassName(device t)
        {
            DeviceId = t.device_id;
            C1Name = t.c1_name;
            C2Name = t.c2_name;
            C3Name = t.c3_name;
            C4Name = t.c4_name;
            C5Name = t.c5_name;
            C6Name = t.c6_name;
            C7Name = t.c7_name;
        }
    }
}
