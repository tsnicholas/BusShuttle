#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.DriverModels;

public class LoopEntryModel
{
    [Required]
    public int Id { get; set; }
    
    public Driver Driver { get; set; }
    
    [Required(ErrorMessage = "Your Driver Id is missing.")]
    [Range(1, int.MaxValue, ErrorMessage = "Driver Id is invalid.")]
    public int DriverId { get; set; } = 1;
    
    public Bus Bus { get; set; }
    
    [Required(ErrorMessage = "Your Bus Id is missing")]
    [Range(1, int.MaxValue, ErrorMessage = "Bus Id is invalid.")]
    public int BusId { get; set; } = 1;
    
    public Loop Loop { get; set; }
    
    [Required(ErrorMessage = "Your Loop Id is missing")]
    [Range(1, int.MaxValue, ErrorMessage = "Loop Id is invalid.")]
    public int LoopId { get; set; } = 1;
    
    public List<Stop> Stops { get; set; } = [];
    
    [Required(ErrorMessage = "Please select a Stop.")]
    [Range(1, int.MaxValue, ErrorMessage = "Stop Id is Invalid.")]
    public int StopId { get; set; } = 1;
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int Boarded {get; set; } = 0;
    
    [Required]
    [Display(Name = "Left Behind")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
    public int LeftBehind { get; set; } = 0;
    
    public static LoopEntryModel CreateModel(int id, Driver driver, Bus bus, Loop loop, List<Stop> stops)
    {
        return new LoopEntryModel
        {
            Id = id,
            Driver = driver,
            DriverId = driver.Id,
            Bus = bus,
            BusId = bus.Id,
            Loop = loop,
            LoopId = loop.Id,
            Stops = stops
        };
    }
}
