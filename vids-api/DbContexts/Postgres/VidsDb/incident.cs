using System;
using System.Collections.Generic;

namespace Vids.DbContexts.Postgres.VidsDb;

public partial class incident
{
    public string id { get; set; } = null!;

    public string? device_id { get; set; }

    public string? lane_id { get; set; }

    public string? incident_type { get; set; }

    public DateTime? start_time { get; set; }

    public DateTime? end_time { get; set; }

    public bool? footage { get; set; }

    public string? owner_id { get; set; }
}
