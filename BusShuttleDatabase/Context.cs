using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using BusShuttleModel;
namespace BusShuttleDatabase;

public class BusShuttleContext : DbContext
{
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Loop> Loops { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public string DbPath { get; }

    public BusShuttleContext() {
        var mainDirectory = Path.GetFullPath("..");
        DbPath = @$"{mainDirectory}\BusShuttleDatabase\BusShuttle.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
}
