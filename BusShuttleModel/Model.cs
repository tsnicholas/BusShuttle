using System.ComponentModel.DataAnnotations;
namespace BusShuttleModel;

public abstract class IBusData(int id) : object
{
    [Key] public int Id { get; set; } = id;

    public abstract void Update(IBusData data);
}

public class Bus(int id, int busNumber) : IBusData(id)
{
    [Required]
    public int BusNumber { get; set; } = busNumber;
    public virtual List<Entry> Entries { get; set; } = [];

    public void AddEntry(Entry entry)
    {
        Entries.Add(entry);
    }

    public override void Update(IBusData data)
    {
        Bus updatedBus = data as Bus ?? throw new InvalidOperationException();
        BusNumber = updatedBus.BusNumber;
    }
}

public class Driver(int id, string firstName, string lastName, string email) : IBusData(id)
{
    [Required]
    public string FirstName { get; set; } = firstName;
    [Required]
    public string LastName { get; set; } = lastName;
    [Required]
    public string Email { get; set; } = email;
    public virtual List<Entry> Entries { get; set; } = [];

    public void AddEntry(Entry entry)
    {
        Entries.Add(entry);
    }

    public override void Update(IBusData data)
    {
        Driver updatedDriver = data as Driver ?? throw new InvalidOperationException();
        FirstName = updatedDriver.FirstName;
        LastName = updatedDriver.LastName;
    }
}

public class Stop(int id, string name, double latitude, double longitude) : IBusData(id)
{
    [Required]
    public string Name { get; set; } = name;
    [Required]
    public double Latitude { get; set; } = latitude;
    [Required]
    public double Longitude { get; set; } = longitude;
    public virtual BusRoute? Route { get; set; }
    public virtual List<Entry> Entries { get; set; } = [];

    public Stop SetRoute(BusRoute route)
    {
        Route = route;
        return this;
    }

    public void AddEntry(Entry entry)
    {
        Entries.Add(entry);
    }

    public override void Update(IBusData data)
    {
        Stop updatedStop = data as Stop ?? throw new InvalidOperationException();
        Name = updatedStop.Name;
        Latitude = updatedStop.Latitude;
        Longitude = updatedStop.Longitude;
    }
}

public class BusRoute(int id, int order) : IBusData(id)
{
    [Required]
    public int Order { get; set; } = order;
    public int StopId { get; set; }
    public virtual Stop? Stop { get; set; }
    public virtual Loop? Loop { get; set; }

    public BusRoute SetLoop(Loop loop)
    {
        Loop = loop;
        return this;
    }

    public BusRoute SetStop(Stop stop)
    {
        Stop = stop;
        StopId = stop.Id;
        return this;
    }

    public override void Update(IBusData data)
    {
        BusRoute updatedRoute = data as BusRoute ?? throw new InvalidOperationException();
        Order = updatedRoute.Order;
    }
}

public class Loop(int id, string name) : IBusData(id)
{
    [Required]
    public string Name { get; set; } = name;
    public virtual List<BusRoute> Routes { get; set; } = [];
    public virtual List<Entry> Entries { get; set; } = [];

    public void AddRoute(BusRoute route)
    {
        Routes.Add(route);
    }

    public void AddEntry(Entry entry)
    {
        Entries.Add(entry);
    }

    public override void Update(IBusData data)
    {
        Loop updatedLoop = data as Loop ?? throw new InvalidOperationException();
        Name = updatedLoop.Name;
    }
}

public class Entry(int id, int boarded, int leftBehind) : IBusData(id)
{
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.Now;
    [Required]
    public int Boarded { get; set; } = boarded;
    [Required]
    public int LeftBehind { get; set; } = leftBehind;
    public virtual Bus? Bus { get; set; }
    public int? BusId { get; set; }
    public virtual Driver? Driver { get; set; }
    public int? DriverId { get; set; }
    public virtual Loop? Loop { get; set; }
    public int? LoopId { get; set; }
    public virtual Stop? Stop { get; set; }
    public int? StopId { get; set; }

    public override void Update(IBusData data)
    {
        Entry updatedEntry = data as Entry ?? throw new InvalidOperationException();
        Timestamp = updatedEntry.Timestamp;
        Boarded = updatedEntry.Boarded;
        LeftBehind = updatedEntry.LeftBehind;
        Bus = updatedEntry.Bus;
        BusId = updatedEntry.BusId;
        Driver = updatedEntry.Driver;
        DriverId = updatedEntry.DriverId;
        Loop = updatedEntry.Loop;
        LoopId = updatedEntry.LoopId;
        Stop = updatedEntry.Stop;
        StopId = updatedEntry.StopId;
    }

    public Entry SetBus(Bus bus)
    {
        Bus = bus;
        BusId = bus.Id;
        return this;
    }

    public Entry SetDriver(Driver driver)
    {
        Driver = driver;
        DriverId = driver.Id;
        return this;
    }

    public Entry SetLoop(Loop loop)
    {
        Loop = loop;
        LoopId = loop.Id;
        return this;
    }

    public Entry SetStop(Stop stop)
    {
        Stop = stop;
        StopId = stop.Id;
        return this;
    }
}
