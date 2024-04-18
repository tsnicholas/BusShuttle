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
    public List<Entry> Entries { get; set; } = [];

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
    public List<Entry> Entries { get; set; } = [];

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
    [Required]
    public BusRoute? Route { get; set; }
    public List<Entry> Entries { get; set; } = [];

    public void SetRoute(BusRoute route)
    {
        Route = route;
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
    [Required]
    public int StopId { get; set; }
    public Stop? Stop { get; set; }
    public Loop? Loop { get; set; }

    public void SetLoop(Loop loop)
    {
        Loop = loop;
    }

    public void SetStop(Stop stop)
    {
        Stop = stop;
    }

    public override void Update(IBusData data)
    {
        BusRoute updatedRoute = data as BusRoute ?? throw new InvalidOperationException();
        Order = updatedRoute.Order;
        Stop = updatedRoute.Stop;
        StopId = updatedRoute.StopId;
    }
}

public class Loop(int id, string name) : IBusData(id)
{
    [Required]
    public string Name { get; set; } = name;
    public List<BusRoute> Routes { get; set; } = [];
    public List<Entry> Entries { get; set; } = [];

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
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public int Boarded { get; set; } = boarded;
    public int LeftBehind { get; set; } = leftBehind;
    [Required]
    public Bus? Bus { get; set; }
    [Required]
    public int? BusId { get; set; }
    [Required]
    public Driver? Driver { get; set; }
    [Required]
    public int? DriverId { get; set; }
    [Required]
    public Loop? Loop { get; set; }
    [Required]
    public int? LoopId { get; set; }
    [Required]
    public Stop? Stop { get; set; }
    [Required]
    public int? StopId { get; set; }

    public override void Update(IBusData data)
    {
        Entry updatedEntry = data as Entry ?? throw new InvalidOperationException();
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

    public void SetBus(Bus bus)
    {
        Bus = bus;
        BusId = bus.Id;
    }

    public void SetDriver(Driver driver)
    {
        Driver = driver;
        DriverId = driver.Id;
    }

    public void SetLoop(Loop loop)
    {
        Loop = loop;
        LoopId = loop.Id;
    }

    public void SetStop(Stop stop)
    {
        Stop = stop;
        StopId = stop.Id;
    }
}
