using System;
using System.Collections.Generic;

namespace Vids.DbContexts.Postgres.VidsDb;

public partial class device
{
    public string device_id { get; set; } = null!;

    public string? device_name { get; set; }

    public string? device_name_2 { get; set; }

    public string? device_tag { get; set; }

    public string? ip_address { get; set; }

    public string? location { get; set; }

    public string? bound { get; set; }

    public decimal? km { get; set; }

    public decimal? latitude { get; set; }

    public decimal? longitude { get; set; }

    public string? control_room { get; set; }

    public int? total_lane { get; set; }

    public string? lane1_id { get; set; }

    public string? lane1_name { get; set; }

    public string? lane2_id { get; set; }

    public string? lane2_name { get; set; }

    public string? lane3_id { get; set; }

    public string? lane3_name { get; set; }

    public string? lane4_id { get; set; }

    public string? lane4_name { get; set; }

    public string? lane5_id { get; set; }

    public string? lane5_name { get; set; }

    public string? lane6_id { get; set; }

    public string? lane6_name { get; set; }

    public string? congestion_line { get; set; }

    public string? camera_id { get; set; }

    public int? free_flow_speed { get; set; }

    public int? road_capacity { get; set; }

    public int? total_class { get; set; }

    public string? c1_name { get; set; }

    public string? c2_name { get; set; }

    public string? c3_name { get; set; }

    public string? c4_name { get; set; }

    public string? c5_name { get; set; }

    public string? c6_name { get; set; }

    public string? c7_name { get; set; }

    public bool? has_speed { get; set; }

    public bool? has_headway { get; set; }

    public bool? has_occupancy { get; set; }

    public bool? has_gap { get; set; }

    public bool? has_flow_rate { get; set; }

    public bool? has_c1_flow_rate { get; set; }

    public bool? has_c2_flow_rate { get; set; }

    public bool? has_c3_flow_rate { get; set; }

    public bool? has_c4_flow_rate { get; set; }

    public bool? has_c5_flow_rate { get; set; }

    public bool? has_c6_flow_rate { get; set; }

    public bool? has_c7_flow_rate { get; set; }

    public bool? has_los { get; set; }

    public string? owner_id { get; set; }

    public DateTime? created_time { get; set; }
}
