using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Vids.DbContexts.Postgres.VidsDb;

public partial class VidsDbContext : DbContext
{
    public VidsDbContext(DbContextOptions<VidsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<device> device { get; set; }

    public virtual DbSet<incident> incident { get; set; }

    public virtual DbSet<traffic_data> traffic_data { get; set; }

    public virtual DbSet<vehicle> vehicle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<device>(entity =>
        {
            entity.HasKey(e => e.device_id).HasName("device_pk");

            entity.Property(e => e.device_id).HasColumnType("character varying");
            entity.Property(e => e.bound).HasColumnType("character varying");
            entity.Property(e => e.c1_name).HasColumnType("character varying");
            entity.Property(e => e.c2_name).HasColumnType("character varying");
            entity.Property(e => e.c3_name).HasColumnType("character varying");
            entity.Property(e => e.c4_name).HasColumnType("character varying");
            entity.Property(e => e.c5_name).HasColumnType("character varying");
            entity.Property(e => e.c6_name).HasColumnType("character varying");
            entity.Property(e => e.c7_name).HasColumnType("character varying");
            entity.Property(e => e.camera_id).HasColumnType("character varying");
            entity.Property(e => e.congestion_line).HasColumnType("character varying");
            entity.Property(e => e.control_room).HasColumnType("character varying");
            entity.Property(e => e.device_name).HasColumnType("character varying");
            entity.Property(e => e.device_name_2).HasColumnType("character varying");
            entity.Property(e => e.device_tag).HasColumnType("character varying");
            entity.Property(e => e.ip_address).HasColumnType("character varying");
            entity.Property(e => e.lane1_id).HasColumnType("character varying");
            entity.Property(e => e.lane1_name).HasColumnType("character varying");
            entity.Property(e => e.lane2_id).HasColumnType("character varying");
            entity.Property(e => e.lane2_name).HasColumnType("character varying");
            entity.Property(e => e.lane3_id).HasColumnType("character varying");
            entity.Property(e => e.lane3_name).HasColumnType("character varying");
            entity.Property(e => e.lane4_id).HasColumnType("character varying");
            entity.Property(e => e.lane4_name).HasColumnType("character varying");
            entity.Property(e => e.lane5_id).HasColumnType("character varying");
            entity.Property(e => e.lane5_name).HasColumnType("character varying");
            entity.Property(e => e.lane6_id).HasColumnType("character varying");
            entity.Property(e => e.lane6_name).HasColumnType("character varying");
            entity.Property(e => e.location).HasColumnType("character varying");
            entity.Property(e => e.owner_id).HasColumnType("character varying");
        });

        modelBuilder.Entity<incident>(entity =>
        {
            entity.HasKey(e => e.id).HasName("incident_pk");

            entity.Property(e => e.id).HasColumnType("character varying");
            entity.Property(e => e.device_id).HasColumnType("character varying");
            entity.Property(e => e.incident_type).HasColumnType("character varying");
            entity.Property(e => e.lane_id).HasColumnType("character varying");
            entity.Property(e => e.owner_id).HasColumnType("character varying");
        });

        modelBuilder.Entity<traffic_data>(entity =>
        {
            entity.HasKey(e => e.id).HasName("traffic_data_pk");

            entity.Property(e => e.id).HasColumnType("character varying");
            entity.Property(e => e.device_id).HasColumnType("character varying");
            entity.Property(e => e.lane_id).HasColumnType("character varying");
            entity.Property(e => e.los).HasColumnType("character varying");
            entity.Property(e => e.owner_id).HasColumnType("character varying");
        });

        modelBuilder.Entity<vehicle>(entity =>
        {
            entity.HasKey(e => e.id).HasName("vehicle_pk");

            entity.Property(e => e.id).HasColumnType("character varying");
            entity.Property(e => e._class)
                .HasColumnType("character varying")
                .HasColumnName("class");
            entity.Property(e => e.device_id).HasColumnType("character varying");
            entity.Property(e => e.lane_id).HasColumnType("character varying");
            entity.Property(e => e.owner_id).HasColumnType("character varying");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
