#nullable disable

using BusShuttleModel;
namespace App.Models.Driver;

public class LoopEntryModel
{
    public BusShuttleModel.Driver BusDriver { get; set; }
    public Bus TheBus { get; set; }
    public Loop BusLoop { get; set; }
    public List<Stop> Stops { get; set; } = new();

    public static LoopEntryModel CreateModel(BusShuttleModel.Driver driver, Bus bus, Loop loop, List<Stop> stops)
    {
        return new LoopEntryModel
        {
            BusDriver = driver,
            TheBus = bus,
            BusLoop = loop,
            Stops = stops 
        };
    }
}
