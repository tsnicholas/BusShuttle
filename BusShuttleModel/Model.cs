namespace BusShuttleModel;

public class Bus
{
    public int Id { get; set; }
    public int BusNumber { get; set; }
}

public class Driver
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Route
{
    public int Id { get; set; }
    public int Order { get; set; }
}

public class Stop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int RouteId { get; set; }
}

public class Loop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Route> Routes { get; set; } 
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
}
