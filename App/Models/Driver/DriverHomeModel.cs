using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.DriverModels;

public class DriverHomeModel
{
    [Required]
    public List<Loop> Loops { get; set; } = [];
    
    [Required]
    public List<Bus> Buses { get; set; } = [];
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Loop Id.")]
    [Display(Name = "Loop")]
    public int LoopId { get; set; } = -1;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Bus Id.")]
    [Display(Name = "Bus Number")]
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
