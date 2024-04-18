using Microsoft.EntityFrameworkCore;
using BusShuttleModel;
namespace BusShuttleDatabase;

public abstract class IBusShuttleContext : DbContext
{
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<BusRoute> Routes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Loop> Loops { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public string DbPath { get; }

    public IBusShuttleContext() {
        var mainDirectory = Path.GetFullPath("..");
        DbPath = @$"{mainDirectory}\BusShuttleDatabase\BusShuttle.db";
    }

    protected abstract override void OnConfiguring(DbContextOptionsBuilder options);
    protected abstract override void OnModelCreating(ModelBuilder modelBuilder);
}

public class BusShuttleContext : IBusShuttleContext
{
    public BusShuttleContext(): base() {}

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bus>(busConfig => {
            busConfig.HasKey(bus => bus.Id).HasName("PrimaryKey_Id");
            busConfig.HasMany(bus => bus.Entries).WithOne(entry => entry.Bus);
        });
        modelBuilder.Entity<Driver>(driverConfig => {
            driverConfig.HasKey(driver => driver.Id).HasName("PrimaryKey_Id");
            driverConfig.HasMany(driver => driver.Entries).WithOne(entry => entry.Driver);
        });
        modelBuilder.Entity<BusRoute>(routeConfig => {
            routeConfig.HasKey(route => route.Id).HasName("PrimaryKey_Id");
            routeConfig.HasOne(route => route.Stop).WithOne(stop => stop.Route).HasForeignKey<BusRoute>(route => route.StopId).IsRequired();
            routeConfig.HasOne(route => route.Loop).WithMany(loop => loop.Routes);
        });
        modelBuilder.Entity<Stop>(stopConfig => {
            stopConfig.HasKey(stop => stop.Id).HasName("PrimaryKey_Id");
            stopConfig.HasOne(stop => stop.Route).WithOne(route => route.Stop).IsRequired();
            stopConfig.HasMany(stop => stop.Entries).WithOne(entry => entry.Stop);
        });
        modelBuilder.Entity<Loop>(loopConfig => {
            loopConfig.HasKey(loop => loop.Id).HasName("PrimaryKey_Id");
            loopConfig.HasMany(loop => loop.Routes).WithOne(route => route.Loop);
            loopConfig.HasMany(loop => loop.Entries).WithOne(entry => entry.Loop);
        });
        modelBuilder.Entity<Entry>(entryConfig => {
            entryConfig.HasKey(entry => entry.Id).HasName("PrimaryKey_Id");
            entryConfig.HasOne(entry => entry.Bus).WithMany(bus => bus.Entries).HasForeignKey(entry => entry.BusId).IsRequired();
            entryConfig.HasOne(entry => entry.Driver).WithMany(driver => driver.Entries).HasForeignKey(entry => entry.DriverId).IsRequired();
            entryConfig.HasOne(entry => entry.Loop).WithMany(loop => loop.Entries).HasForeignKey(entry => entry.LoopId).IsRequired();
            entryConfig.HasOne(entry => entry.Stop).WithMany(stop => stop.Entries).HasForeignKey(entry => entry.StopId).IsRequired();
        });
    }
}
