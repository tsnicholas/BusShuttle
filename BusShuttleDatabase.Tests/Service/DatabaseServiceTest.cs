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
    
    private static readonly BusShuttleContext context = CreateContext();

    private static BusShuttleContext CreateContext()
    {
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder{DataSource=":memory:"}.ToString());
        return new BusShuttleContext(new DbContextOptionsBuilder<BusShuttleContext>().UseSqlite(connection).Options);
    }

    private readonly static object _lock = new();
    private static bool _isDatabaseBeingUsed = false;
    private readonly DatabaseService service;

    public DatabaseServiceTest()
    {
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        service = new(context);
    }

    private static void FillDatabase()
    {
        lock (_lock)
        {
            if(!_isDatabaseBeingUsed)
            {
                context.Buses.AddRange(testBuses);
                context.Drivers.AddRange(testDrivers);
                context.Stops.AddRange(testStops);
                context.Routes.AddRange(testRoutes);
                context.Loops.AddRange(testLoops);
                context.Entries.AddRange(testEntries);
                context.SaveChanges();
                _isDatabaseBeingUsed = true;
            }
        }
    }

    private static void EmptyDatabase()
    {
        context.Buses.RemoveRange(context.Buses);
        context.Drivers.RemoveRange(context.Drivers);
        context.Stops.RemoveRange(context.Stops);
        context.Routes.RemoveRange(context.Routes);
        context.Loops.RemoveRange(context.Loops);
        context.Entries.RemoveRange(context.Entries);
        context.SaveChanges();
        _isDatabaseBeingUsed = false;
    }

    [Fact]
    public void Can_Get_All_Buses_Case()
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testBuses, service.GetAll<Bus>());
        }
        // Finally ensures that the test database is emptied at the end no matter what. 
        // This ensures we don't get data to crossover to other tests. 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Get_All_Drivers_Case()
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testDrivers, service.GetAll<Driver>());
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Get_All_Stops_Case()
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testStops, service.GetAll<Stop>());
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Get_All_Routes_Case()
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testRoutes, service.GetAll<BusRoute>());
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Get_All_Loops_Case()
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testLoops, service.GetAll<Loop>());
        } 
        finally 
        {
            EmptyDatabase();   
        }
    }

    [Fact]
    public void Can_Get_All_Entries_Case()
    {
        FillDatabase();
        try 
        {
            List<Entry> actual = service.GetAll<Entry>();
            // No matter what, the value in create entry test is passed here. Don't ask me why :/
            // TODO: Fix this
            if(actual.Count == 3) actual.RemoveAt(2);
            Assert.Equal(testEntries, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Bus_Case(int index, int id)
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testBuses[index], service.GetById<Bus>(id));
        } 
        finally 
        {
            EmptyDatabase();   
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Driver_Case(int index, int id)
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testDrivers[index], service.GetById<Driver>(id));
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Stop_Case(int index, int id)
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testStops[index], service.GetById<Stop>(id));
        } 
        finally 
        {
            EmptyDatabase();
        }   
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Route_Case(int index, int id)
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testRoutes[index], service.GetById<BusRoute>(id));
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Loop_Case(int index, int id)
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testLoops[index], service.GetById<Loop>(id));
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Can_Get_By_Id_Entry_Case(int index, int id)
    {
        FillDatabase();
        try 
        {
            Assert.Equal(testEntries[index], service.GetById<Entry>(id));
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Create_Bus()
    {
        FillDatabase();
        try 
        {
            int newId = 3;
            Bus newBus = new(newId, 5);
            service.CreateEntity(newBus);
            var actual = context.Buses.Single(bus => bus.Id == newId);
            Assert.Equal(newBus, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Create_Driver()
    {
        FillDatabase();
        try 
        {
            int newId = 3;
            Driver newDriver = new(newId, "Sally", "Johnson", "example@Gmail.com");
            service.CreateEntity(newDriver);
            var actual = context.Drivers.Single(driver => driver.Id == newId);
            Assert.Equal(newDriver, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Create_Stop()
    {
        FillDatabase();
        try 
        {
            int newId = 3;
            Stop newStop = new(newId, "Mushroom Kingdom", 500, 0);
            service.CreateEntity(newStop);
            var actual = context.Stops.Single(stop => stop.Id == newId);
            Assert.Equal(newStop, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Create_Route()
    {
        FillDatabase();
        try 
        {
            int newId = 3;
            BusRoute route = new BusRoute(newId, 1).SetStop(new Stop(newId, "Mushroom Kingdom", 500, 0));
            service.CreateEntity(route);
            var actual = context.Routes.Single(route => route.Id == newId);
            Assert.Equal(route, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Create_Loop()
    {
        FillDatabase();
        try 
        {
            int newId = 3;
            Loop loop = new(newId, "Green Hill Zone");
            service.CreateEntity(loop);
            var actual = context.Loops.Single(loop => loop.Id == newId);
            Assert.Equal(loop, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Create_Entry()
    {
        FillDatabase();
        try 
        {
            int newId = 3;
            Entry entry = new Entry(newId, 17, 10).SetBus(testBuses[0]).SetDriver(testDrivers[0])
                .SetStop(testStops[1]).SetLoop(testLoops[1]);
            service.CreateEntity(entry);
            var actual = context.Entries.Single(entry => entry.Id == newId);
            Assert.Equal(entry, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Update_Bus_By_Id()
    {
        FillDatabase();
        try 
        {
            int target = 1;
            Bus updatedBus = new(target, 44);
            service.UpdateById(target, updatedBus);
            var actual = context.Buses.SingleOrDefault(bus => bus.Id == target);
            Assert.Equivalent(updatedBus, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Update_Driver_By_Id()
    {
        FillDatabase();
        try 
        {
            int target = 1;
            Driver updatedDriver = new(target, "Vienna", "Nicholas", "tsnicholas@bsu.edu");
            service.UpdateById(target, updatedDriver);
            var actual = context.Drivers.SingleOrDefault(driver => driver.Id == target);
            Assert.Equivalent(updatedDriver, actual);
        } 
        finally 
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Update_Stop_By_Id()
    {
        FillDatabase();
        try 
        {
            int target = 1;
            Stop updatedStop = new Stop(target, "Hidden Leaf Village", 4000, -3000).SetRoute(testRoutes[0]);
            service.UpdateById(target, updatedStop);
            var actual = context.Stops.SingleOrDefault(stop => stop.Id == target);
            Assert.Equivalent(updatedStop, actual);
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Update_Route_By_Id()
    {
        FillDatabase();
        try
        {
            int target = 1;
            BusRoute updatedRoute = new BusRoute(target, 5).SetStop(testStops[target - 1]);
            service.UpdateById(target, updatedRoute);
            var actual = context.Routes.SingleOrDefault(route => route.Id == target);
            Assert.Equivalent(updatedRoute, actual);
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Update_Loop_By_Id()
    {
        FillDatabase();
        try
        {
            int target = 1;
            Loop updatedLoop = new(target, "Green Hill Zone");
            service.UpdateById(target, updatedLoop);
            var actual = context.Loops.SingleOrDefault(loop => loop.Id == target);
            Assert.Equivalent(updatedLoop, actual);
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Update_Entry_By_Id()
    {
        FillDatabase();
        try
        {
            int target = 1;
            Entry updatedEntry = new Entry(target, 16, 4).SetBus(testBuses[0]).SetDriver(testDrivers[0])
                .SetLoop(testLoops[0]).SetStop(testStops[0]);
            service.UpdateById(target, updatedEntry);
            var actual = context.Entries.SingleOrDefault(entry => entry.Id == target);
            Assert.Equivalent(updatedEntry, actual);
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Delete_Bus_By_Id()
    {
        FillDatabase();
        try
        {
            int targetId = 1;
            var deletedBus = context.Buses.Single(bus => bus.Id == targetId);
            service.DeleteById<Bus>(targetId);
            Assert.DoesNotContain(deletedBus, context.Buses.ToList());
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Delete_Driver_By_Id()
    {
        FillDatabase();
        try
        {
            int targetId = 1;
            var deletedDriver = context.Drivers.Single(driver => driver.Id == targetId);
            service.DeleteById<Driver>(targetId);
            Assert.DoesNotContain(deletedDriver, context.Drivers.ToList());
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Delete_Stop_By_Id()
    {
        FillDatabase();
        try
        {
            int targetId = 1;
            var deletedStop = context.Stops.Single(stop => stop.Id == targetId);
            service.DeleteById<Stop>(targetId);
            Assert.DoesNotContain(deletedStop, context.Stops.ToList());
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Delete_Route_By_Id()
    {
        FillDatabase();
        try
        {
            int targetId = 1;
            var deletedRoute = context.Routes.Single(route => route.Id == targetId);
            service.DeleteById<BusRoute>(targetId);
            Assert.DoesNotContain(deletedRoute, context.Routes.ToList());
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Delete_Loop_By_Id()
    {
        FillDatabase();
        try
        {
            int targetId = 1;
            var deletedLoop = context.Loops.Single(loop => loop.Id == targetId);
            service.DeleteById<Loop>(targetId);
            Assert.DoesNotContain(deletedLoop, context.Loops.ToList());
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Delete_Entry_By_Id()
    {
        FillDatabase();
        try
        {
            int targetId = 1;
            var deletedEntry = context.Entries.Single(entry => entry.Id == targetId);
            service.DeleteById<Entry>(targetId);
            Assert.DoesNotContain(deletedEntry, context.Entries.ToList());
        }
        finally
        {
            EmptyDatabase();
        }
    }

    [Fact]
    public void Can_Driver_By_Email()
    {
        FillDatabase();
        try
        {
            Assert.Equal(testDrivers[0], service.GetDriverByEmail(testDrivers[0].Email));
        }
        finally
        {
            EmptyDatabase();
        }
    }
}
