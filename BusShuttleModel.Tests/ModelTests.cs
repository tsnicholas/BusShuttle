namespace BusShuttleModel.Tests;

public class BusTests
{
    private static readonly int id = 1;
    private static readonly int busNumber = 12;

    [Fact]
    public void CreationTest()
    {
        Bus bus = new(id, busNumber);
        Assert.Equal(id, bus.Id);
        Assert.Equal(busNumber, bus.BusNumber);
    }

    [Fact]
    public void UpdateTest()
    {
        const int updatedBusNumber = 42;
        Bus bus = new(id, busNumber);
        Bus updatedBus = new(id, updatedBusNumber);
        bus.Update(updatedBus);
        Assert.Equal(updatedBusNumber, bus.BusNumber);
    }
}

public class DriverTests
{
    private static readonly int id = 1;
    private static readonly string firstName = "Tim";
    private static readonly string lastName = "Nicholas";
    private static readonly string email = "tsnicholas@bsu.edu";

    [Fact]
    public void Creation_Successful()
    {
        Driver driver = new(id, firstName, lastName, email);
        Assert.Equal(id, driver.Id);
        Assert.Equal(firstName, driver.FirstName);
        Assert.Equal(lastName, driver.LastName);
        Assert.Equal(email, driver.Email);
    }

    [Fact]
    public void Update_Successful()
    {
        const string updatedFirstName = "Steve";
        Driver driver = new(id, firstName, lastName, email);
        Driver updatedDriver = new(id, updatedFirstName, lastName, email);
        driver.Update(updatedDriver);
        Assert.Equal(updatedFirstName, driver.FirstName);
    }

    [Fact]
    public void Add_Entry_Successful()
    {
        Driver driver = new(id, firstName, lastName, email);
        Entry firstEntry = new(1, 5, 10);
        driver.AddEntry(firstEntry);
        Assert.Equal([firstEntry], driver.Entries);
    }
}

public class StopTests
{
    private static readonly int id = 1;
    private static readonly string name = "Constantinople";
    private static readonly int latitude = 500;
    private static readonly int longitude = 0;
    [Fact]
    public void Creation_Successful()
    {
        Stop stop = new(id, name, latitude, longitude);
        Assert.Equal(id, stop.Id);
        Assert.Equal(name, stop.Name);
        Assert.Equal(latitude, stop.Latitude);
        Assert.Equal(longitude, stop.Longitude);
    }

    [Fact]
    public void SetRoute_Successful()
    {
        BusRoute route = new(1, 2);
        Stop stop = new Stop(id, name, latitude, longitude).SetRoute(route);
        Assert.Equal(route, stop.Route);
    }

    [Fact]
    public void Update_Successful()
    {
        const string updatedName = "Istanbul";
        Stop stop = new(id, name, latitude, longitude);
        Stop updatedStop = new(id, updatedName, latitude, longitude);
        stop.Update(updatedStop);
        Assert.Equal(updatedName, stop.Name);
    }

    [Fact]
    public void Add_Entry_Successful()
    {
        Stop stop = new(id, name, latitude, longitude);
        Entry firstEntry = new(1, 5, 10);
        stop.AddEntry(firstEntry);
        Assert.Equal([firstEntry], stop.Entries);
    }
}

public class BusRouteTests
{
    private static readonly int id = 1;
    private static readonly int order = 2;
    private static readonly Stop stop = new(3, "Bikini Bottom", 500, -1000);

    [Fact]
    public void Creation_Successful()
    {
        BusRoute route = new(id, order);
        Assert.Equal(id, route.Id);
        Assert.Equal(order, route.Order);
    }

    [Fact]
    public void SetStop_Successful()
    {
        BusRoute route = new BusRoute(id, order).SetStop(stop);
        Assert.Equal(id, route.Id);
        Assert.Equal(order, route.Order);
        Assert.Equal(stop, route.Stop);
    }

    [Fact]
    public void Update_Successful()
    {
        const int newOrder = 0;
        BusRoute route = new(id, order);
        BusRoute updatedRoute = new(id, newOrder);
        route.Update(updatedRoute);
        Assert.Equal(newOrder, route.Order);
    }
}

public class LoopTests
{
    private static readonly int id = 1;
    private static readonly string name = "Green Hill Zone";

    [Fact]
    public void Creation_Successful()
    {
        Loop loop = new(id, name);
        Assert.Equal(id, loop.Id);
        Assert.Equal(name, loop.Name);
    }

    [Fact]
    public void Update_Successful()
    {
        const string updatedName = "Emerald Hill Zone";
        Loop loop = new(id, name);
        Loop updatedLoop = new(id, updatedName);
        loop.Update(updatedLoop);
        Assert.Equal(updatedName, loop.Name);
    }

    [Fact]
    public void AddEntry_Successful()
    {
        Loop loop = new(id, name);
        Entry entry= new(4, 3, 10);
        loop.AddEntry(entry);
        Assert.Contains(entry, loop.Entries);
    }

    [Fact]
    public void AddRoute_Successful()
    {
        Loop loop = new(id, name);
        BusRoute route = new(10, 120);
        loop.AddRoute(route);
        Assert.Contains(route, loop.Routes);
    }
}

public class EntryTests
{
    private static readonly int id = 1;
    private static readonly int boarded = 44;
    private static readonly int leftBehind = 12;
    
    [Fact]
    public void Creation_Successful()
    {
        Entry entry = new(id, boarded, leftBehind);
        Assert.Equal(id, entry.Id);
        Assert.Equal(boarded, entry.Boarded);
        Assert.Equal(leftBehind, entry.LeftBehind);
    }

    [Fact]
    public void Update_Successful()
    {
        const int updatedBoarded = 15;
        Entry entry = new(id, boarded, leftBehind);
        Entry updatedEntry = new(id, updatedBoarded, leftBehind);
        entry.Update(updatedEntry);
        Assert.Equal(updatedBoarded, entry.Boarded);
    }

    [Fact]
    public void SetBus_Successful()
    {
        Bus bus = new(1, 69);
        Entry entry = new Entry(id, boarded, leftBehind).SetBus(bus);
        Assert.Equal(bus, entry.Bus);
        Assert.Equal(bus.Id, entry.BusId);
    }

    [Fact]
    public void SetDriver_Successful()
    {
        Driver driver = new(1, "Michael", "Jackson", "heheWOOOO@yahoo.com");
        Entry entry = new Entry(id, boarded, leftBehind).SetDriver(driver);
        Assert.Equal(driver, entry.Driver);
        Assert.Equal(driver.Id, entry.DriverId);
    }

    [Fact]
    public void SetStop_Successful()
    {
        Stop stop = new(1, "Aperture Science", 500, 100);
        Entry entry = new Entry(id, boarded, leftBehind).SetStop(stop);
        Assert.Equal(stop, entry.Stop);
        Assert.Equal(stop.Id, entry.StopId);
    }

    [Fact]
    public void SetLoop_Successful()
    {
        Loop loop = new(1, "Green Loop");
        Entry entry = new Entry(id, boarded, leftBehind).SetLoop(loop);
        Assert.Equal(loop, entry.Loop);
        Assert.Equal(loop.Id, entry.LoopId);
    }
}
