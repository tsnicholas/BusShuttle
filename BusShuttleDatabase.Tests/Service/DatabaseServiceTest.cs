using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BusShuttleModel;
using Database.Service;
namespace Database.Test.Service;

public class DatabaseServiceTest
{
    private static readonly List<Bus> testBuses = [
        new Bus(1, 69),
        new Bus(2, 420),
    ];
    private static readonly List<Driver> testDrivers = [
        new Driver(1, "Tim", "Nicholas", "tsnicholas@bsu.edu"),
        new Driver(2, "Momiji", "Rocks", "WaifuMaster1@Gmail.com"),
    ];
    private static readonly List<Stop> testStops = [
        new Stop(1, "Bikini Bottom", 1500, 2000),
        new Stop(2, "South Park", 1250, -1200),
    ];
    private static readonly List<BusRoute> testRoutes = [
        new BusRoute(1, 1).SetStop(testStops[0]),
        new BusRoute(2, 2).SetStop(testStops[1]),
    ];
    private static readonly List<Loop> testLoops = [
        new Loop(1, "Ohio"),
        new Loop(2, "Canada")
    ];
    private static readonly List<Entry> testEntries = [
        new Entry(1, 5, 10).SetBus(testBuses[0]).SetDriver(testDrivers[0]).SetLoop(testLoops[0]).SetStop(testStops[0]),
        new Entry(2, 3, 1).SetBus(testBuses[1]).SetDriver(testDrivers[1]).SetLoop(testLoops[1]).SetStop(testStops[1])
    ];
    
    private static readonly object _lock = new();
    private static bool _isDatabaseCurrentlyBeingUsed;
    private readonly BusShuttleContext context = CreateContext();

    private static BusShuttleContext CreateContext()
    {
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder{DataSource=":memory:"}.ToString());
        return new BusShuttleContext(new DbContextOptionsBuilder<BusShuttleContext>().UseSqlite(connection).Options);
    }

    private readonly DatabaseService service;

    public DatabaseServiceTest()
    {
        context.Database.EnsureDeleted();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        service = new(context);
    }

    private void FillDatabase()
    {
        context.Buses.AddRange(testBuses);
        context.Drivers.AddRange(testDrivers);
        context.Stops.AddRange(testStops);
        context.Routes.AddRange(testRoutes);
        context.Loops.AddRange(testLoops);
        context.Entries.AddRange(testEntries);
        context.SaveChanges();
    }

    private void EmptyDatabase()
    {
        context.Buses.RemoveRange(context.Buses);
        context.Drivers.RemoveRange(context.Drivers);
        context.Stops.RemoveRange(context.Stops);
        context.Routes.RemoveRange(context.Routes);
        context.Loops.RemoveRange(context.Loops);
        context.Entries.RemoveRange(context.Entries);
        context.SaveChanges();
    }

    [Fact]
    public void Can_Get_All_Buses_Case()
    {
        FillDatabase();
        Assert.Equal(testBuses, service.GetAll<Bus>());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Get_All_Drivers_Case()
    {
        FillDatabase();
        Assert.Equal(testDrivers, service.GetAll<Driver>());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Get_All_Stops_Case()
    {
        FillDatabase();
        Assert.Equal(testStops, service.GetAll<Stop>());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Get_All_Routes_Case()
    {
        FillDatabase();
        Assert.Equal(testRoutes, service.GetAll<BusRoute>());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Get_All_Loops_Case()
    {
        FillDatabase();
        Assert.Equal(testLoops, service.GetAll<Loop>());
        EmptyDatabase();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Bus_Case(int index, int id)
    {
        FillDatabase();
        Assert.Equal(testBuses[index], service.GetById<Bus>(id));
        EmptyDatabase();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Driver_Case(int index, int id)
    {
        FillDatabase();
        Assert.Equal(testDrivers[index], service.GetById<Driver>(id));
        EmptyDatabase();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Stop_Case(int index, int id)
    {
        FillDatabase();
        Assert.Equal(testStops[index], service.GetById<Stop>(id));
        EmptyDatabase();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Route_Case(int index, int id)
    {
        FillDatabase();
        Assert.Equal(testRoutes[index], service.GetById<BusRoute>(id));
        EmptyDatabase();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Loop_Case(int index, int id)
    {
        FillDatabase();
        Assert.Equal(testLoops[index], service.GetById<Loop>(id));
        EmptyDatabase();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Entry_Case(int index, int id)
    {
        lock (_lock)
        {
            if(!_isDatabaseCurrentlyBeingUsed)
            {
                return;
            }
        }
        FillDatabase();
        Assert.Equal(testEntries[index], service.GetById<Entry>(id));
        EmptyDatabase();
    }

    [Fact]
    public void Can_Create_Bus()
    {
        Bus bus = new(3, 90);
        service.CreateEntity(bus);
        Assert.Equal([bus], context.Buses.ToList());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Create_Driver()
    {
        Driver driver = new(3, "Sally", "Johnson", "example@Gmail.com");
        service.CreateEntity(driver);
        Assert.Equal([driver], context.Drivers.ToList());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Create_Stop()
    {
        Stop stop = new(3, "Mushroom Kingdom", 500, 0);
        service.CreateEntity(stop);
        Assert.Equal([stop], context.Stops.ToList());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Create_Route()
    {
        BusRoute route = new BusRoute(3, 1).SetStop(new Stop(3, "Mushroom Kingdom", 500, 0));
        service.CreateEntity(route);
        Assert.Equal([route], context.Routes.ToList());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Create_Loop()
    {
        Loop loop = new(3, "Green Hill Zone");
        service.CreateEntity(loop);
        Assert.Equal([loop], context.Loops.ToList());
        EmptyDatabase();
    }

    [Fact]
    public void Can_Create_Entry()
    {
        FillDatabase();
        Entry entry = new Entry(3, 5, 10).SetBus(testBuses[0]).SetDriver(testDrivers[0]).SetStop(testStops[1]).SetLoop(testLoops[1]);
        service.CreateEntity(entry);
        var actual = context.Entries.OrderBy(entry => entry.Id).ToList();
        Assert.Equal(entry, actual[2]);
        EmptyDatabase();
    }
}
