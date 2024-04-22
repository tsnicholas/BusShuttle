#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;

namespace App.Models.Manager;

public class CreateEntryModel
{
    [Key] 
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Please enter the number boarded.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int Boarded { get; set; }
    
    [Required(ErrorMessage = "Please enter the number left behind.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int LeftBehind { get; set; }
    
    [Required]
    public int BusId { get; set; }
    public List<Bus> Buses { get; set; }
    
    [Required]
    public int DriverId { get; set; }
    public List<Driver> Drivers { get; set; }
    
    [Required]
    public int LoopId { get; set; }
    public List<Loop> Loops { get; set; }
    
    [Required]
    public int StopId { get; set; }
    public List<Stop> Stops { get; set; }

    public static CreateEntryModel CreateEntry(int id, List<Bus> buses, List<Driver> drivers, List<Loop> loops, List<Stop> stops)
    {
        return new CreateEntryModel
        {
            Id = id,
            Boarded = 0,
            LeftBehind = 0,
            Buses = buses,
            Drivers = drivers,
            Loops = loops,
            Stops = stops
        };
    }
}
