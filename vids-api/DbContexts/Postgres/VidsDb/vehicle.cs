using System;
using System.Collections.Generic;

namespace Vids.DbContexts.Postgres.VidsDb;

public partial class vehicle
{
    public string id { get; set; } = null!;

    public string? device_id { get; set; }

    public string? lane_id { get; set; }

    public DateTime? passing_time { get; set; }

    public int? speed { get; set; }

    public string? _class { get; set; }

    public string? owner_id { get; set; }
}
