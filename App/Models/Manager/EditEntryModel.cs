#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class EditEntryModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }
    
    [Required(ErrorMessage = "Please enter the number boarded.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int Boarded { get; set; }
    
    [Required(ErrorMessage = "Please enter the number left behind.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int LeftBehind { get; set; }
    
    [Required]
    public int BusId { get; set; }
    public List<Bus> Buses { get; set; } = [];
    
    [Required]
    public int DriverId { get; set; }
    public List<Driver> Drivers { get; set; } = [];
    
    [Required]
    public int LoopId { get; set; }
    public List<Loop> Loops { get; set; } = [];
    
    [Required]
    public int StopId { get; set; }
    public List<Stop> Stops { get; set; } = [];

    public static EditEntryModel FromEntry(Entry entry, List<Bus> buses, List<Driver> drivers, List<Loop> loops, List<Stop> stops)
    {
        return new EditEntryModel
        {
            Id = entry.Id,
            Timestamp = entry.Timestamp,
            Boarded = entry.Boarded,
            LeftBehind = entry.LeftBehind,
            BusId = entry.BusId ?? -1,
            Buses = buses,
            DriverId = entry.DriverId ?? -1,
            Drivers = drivers,
            LoopId = entry.LoopId ?? -1,
            Loops = loops,
            StopId = entry.StopId ?? -1,
            Stops = stops
        };
    }
}
