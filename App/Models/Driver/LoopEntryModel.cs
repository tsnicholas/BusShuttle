#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.DriverModels;

public class LoopEntryModel
{
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "Your Driver Id is missing.")]
    [Range(1, int.MaxValue, ErrorMessage = "Driver Id is invalid.")]
    public int DriverId { get; set; } = 1;
    [Required(ErrorMessage = "Your Bus Id is missing")]
    [Range(1, int.MaxValue, ErrorMessage = "Bus Id is invalid.")]
    public int BusId { get; set; } = 1;
    [Required(ErrorMessage = "Your Loop Id is missing")]
    [Range(1, int.MaxValue, ErrorMessage = "Loop Id is invalid.")]
    public int LoopId { get; set; } = 1;
    public List<Stop> Stops { get; set; } = [];
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Stop Id.")]
    public int StopId { get; set; } = 1;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int Boarded {get; set; } = 0;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int LeftBehind { get; set; } = 0;
    
    public static LoopEntryModel CreateModel(int id, Driver driver, Bus bus, Loop loop, List<Stop> stops)
    {
        return new LoopEntryModel
        {
            Id = id,
            DriverId = driver.Id,
            BusId = bus.Id,
            LoopId = loop.Id,
            Stops = stops
        };
    }
}
