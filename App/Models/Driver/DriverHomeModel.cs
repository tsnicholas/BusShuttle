using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Driver;

public class DriverHomeModel
{
    [Required]
    public List<Loop> Loops { get; set; } = new();
    [Required]
    public List<Bus> Buses { get; set; } = new();
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Loop Id.")]
    public int LoopId { get; set; } = -1;
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Bus Id.")]
    public int BusId { get; set; } = -1;

    public static DriverHomeModel CreateUsingLists(List<Loop> loops, List<Bus> buses)
    {
        return new DriverHomeModel
        {
            Loops = loops,
            Buses = buses
        };
    }
}
