using BusShuttleModel;
using Database.Service;
namespace Database.Test.Service;

public class DatabaseServiceTest : IClassFixture<DatabaseFixture>
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
    private static List<Entry> testEntries = [
        new Entry(1, 5, 10).SetBus(testBuses[0]).SetDriver(testDrivers[0]).SetLoop(testLoops[0]).SetStop(testStops[0]),
        new Entry(2, 3, 1).SetBus(testBuses[1]).SetDriver(testDrivers[1]).SetLoop(testLoops[1]).SetStop(testStops[1])
    ];
    private readonly DatabaseFixture _fixture;
    private readonly BusShuttleContext context;
    private readonly DatabaseService service;

    public DatabaseServiceTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
        Assert.NotNull(_fixture.Context);
        context = _fixture.Context;
        service = new(context);
    }

    [Fact]
    public void Can_Get_All_Buses_Case()
    {
        context.Buses.AddRange(testBuses);
        context.SaveChanges();
        Assert.Equal(testBuses, service.GetAll<Bus>());
    }

    [Fact]
    public void Can_Get_All_Drivers_Case()
    {
        context.Drivers.AddRange(testDrivers);
        context.SaveChanges();
        Assert.Equal(testDrivers, service.GetAll<Driver>());
    }

    [Fact]
    public void Can_Get_All_Stops_Case()
    {
        context.Stops.AddRange(testStops);
        context.SaveChanges();
        Assert.Equal(testStops, service.GetAll<Stop>());
    }
}
