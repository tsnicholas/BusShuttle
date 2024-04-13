using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BusShuttleModel;

public class Bus
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int BusNumber { get; set; }
    public List<Entry>? Entries { get; set; }

    public Bus() {}

    public Bus(int id, int busNumber)
    {
        Id = id;
        BusNumber = busNumber;
    }

    public void Update(int newBusNumber)
    {
        BusNumber = newBusNumber;
    }
}

public class Driver
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    public List<Entry>? Entries { get; set; }

    public Driver() {}

    public Driver(int id, string firstName, string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void Update(string newFirstName, string newLastName)
    {
        FirstName = newFirstName;
        LastName = newLastName;
    }
}

public class Stop
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Latitude { get; set; }
    [Required]
    public double Longitude { get; set; }
    [Required]
    public BusRoute? Route { get; set; }
    public List<Entry>? Entries { get; set; }

    public Stop() {}

    public Stop(int id, string name, double latitude, double longitude)
    {
        Id = id;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void Update(string newName, double newLatitude, double newLongitude)
    {
        Name = newName;
        Latitude = newLatitude;
        Longitude = newLongitude;
    }
}

public class BusRoute
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int Order { get; set; }
    [Required]
    public int StopId { get; set; }
    [Required]
    public Stop Stop { get; set; }
    public Loop? Loop { get; set; }

    public BusRoute(int id, int order)
    {
        Id = id;
        Order = order;
    }

    public void Update(int newOrder)
    {
        Order = newOrder;
    }
}

public class Loop
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public List<BusRoute> Routes { get; set; } = new();
    public List<Entry>? Entries { get; set; }
    
    public Loop() {}
    
    public Loop(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}

public class Entry
{
    [Required]
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    [Required]
    public Bus Bus { get; set; }
    [Required]
    public int BusId { get; set; }
    [Required]
    public Driver Driver { get; set; }
    [Required]
    public int DriverId { get; set; }
    [Required]
    public Loop Loop { get; set; }
    [Required]
    public int LoopId { get; set; }
    [Required]
    public Stop Stop { get; set; }
    [Required]
    public int StopId { get; set; }

    public Entry() {}

    public Entry(int id, int boarded, int leftBehind, Bus bus, Driver driver, Loop loop, Stop stop)
    {
        Id = id;
        Boarded = boarded;
        LeftBehind = leftBehind;
        Bus = bus;
        BusId = bus.Id;
        Driver = driver;
        DriverId = driver.Id;
        Loop = loop;
        LoopId = loop.Id;
        Stop = stop;
        StopId = stop.Id;
    }

    public void Update(DateTime timestamp, int boarded, int leftBehind, Bus bus, Driver driver, Loop loop, Stop stop)
    {
        Timestamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftBehind;
        Bus = bus;
        BusId = bus.Id;
        Driver = driver;
        DriverId = driver.Id;
        Loop = loop;
        LoopId = loop.Id;
        Stop = stop;
        StopId = stop.Id;
    }
}
