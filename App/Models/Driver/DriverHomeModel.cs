#nullable disable

using BusShuttleModel;
namespace App.Models.Driver;

public class DriverHomeModel
{
    public List<Loop> Loops { get; set; }
    public Loop SelectedLoop { get; set; }
    public List<Bus> Buses { get; set; }
    public Bus SelectedBus { get; set; }

    public static DriverHomeModel FromLists(List<Loop> loops, List<Bus> buses)
    {
        return new DriverHomeModel
        {
            Loops = loops,
            SelectedLoop = loops[0],
            Buses = buses,
            SelectedBus = buses[0]
        };
    }
}
