using System;
using System.Collections.Generic;

namespace Vids.DbContexts.Postgres.VidsDb;

public partial class traffic_data
{
    public string id { get; set; } = null!;

    public string? device_id { get; set; }

    public string? lane_id { get; set; }

    public DateTime? traffic_time { get; set; }

    public int? c1 { get; set; }

    public int? c2 { get; set; }

    public int? c3 { get; set; }

    public int? c4 { get; set; }

    public int? c5 { get; set; }

    public int? c6 { get; set; }

    public int? c7 { get; set; }

    public int? total_vol { get; set; }

    public int? flow_rate { get; set; }

    public int? speed { get; set; }

    public decimal? headway { get; set; }

    public string? los { get; set; }

    public decimal? gap { get; set; }

    public int? c1_flow_rate { get; set; }

    public int? c2_flow_rate { get; set; }

    public int? c3_flow_rate { get; set; }

    public int? c4_flow_rate { get; set; }

    public int? c5_flow_rate { get; set; }

    public int? c6_flow_rate { get; set; }

    public int? c7_flow_rate { get; set; }

    public string? owner_id { get; set; }
}
