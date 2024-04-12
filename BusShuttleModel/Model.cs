namespace BusShuttleModel;

public class Bus
{
    public int Id { get; set; }
    public int BusNumber { get; set; }

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
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

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

public class Route
{
    public int Id { get; set; }
    public int Order { get; set; }

    public Route(int id, int order)
    {
        Id = id;
        Order = order;
    }

    public void Update(int newOrder)
    {
        Order = newOrder;
    }
}

public class Stop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int RouteId { get; set; }

    public Stop(int id, string name, double latitude, double longitude, int routeId)
    {
        Id = id;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        RouteId = routeId;
    }

    public void Update(string newName, double newLatitude, double newLongitude, int newRouteId)
    {
        Name = newName;
        Latitude = newLatitude;
        Longitude = newLongitude;
        RouteId = newRouteId;
    }
}

public class Loop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Route> Routes { get; set; } = new();
    
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
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    public int BusId { get; set; }
    public int DriverId { get; set; }
    public int LoopId { get; set; }
    public int StopId { get; set; }

    public Entry(int id, int boarded, int leftBehind, int busId, int driverId, int loopId, int stopId)
    {
        Id = id;
        Timestamp = DateTime.Now;
        Boarded = boarded;
        LeftBehind = leftBehind;
        BusId = busId;
        DriverId = driverId;
        LoopId = loopId;
        StopId = stopId;
    }

    public void Update(DateTime timestamp, int boarded, int leftBehind, int busId, int driverId, int loopId, int stopId)
    {
        Timestamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftBehind;
        BusId = busId;
        DriverId = driverId;
        LoopId = loopId;
        StopId = stopId;
    }
}
