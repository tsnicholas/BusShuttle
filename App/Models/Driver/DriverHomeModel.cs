using BusShuttleModel;
namespace App.Models.Driver;

public class DriverHomeModel
{
    public List<Loop> Loops { get; set; }

    public static DriverHomeModel FromLoops(List<Loop> loops)
    {
        return new DriverHomeModel
        {
            Loops = loops
        };
    }
}
