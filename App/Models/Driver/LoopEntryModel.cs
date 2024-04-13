#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Driver;

public class LoopEntryModel
{
    [Required]
    public BusShuttleModel.Driver BusDriver { get; set; }
    [Required]
    public Bus TheBus { get; set; }
    [Required]
    public Loop BusLoop { get; set; }
    public List<Stop> Stops { get; set; } = new();
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Stop Id.")]
    public int StopId { get; set; } = 1;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int Boarded {get; set; } = 0;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int LeftBehind { get; set; } = 0;

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
