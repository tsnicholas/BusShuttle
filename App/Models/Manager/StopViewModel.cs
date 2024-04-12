#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class StopViewModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Longitude { get; set; }
    [Required]
    public double Latitude { get; set; }
    public int RouteId { get; set; }

    public static StopViewModel FromStop(Stop stop)
    {
        return new StopViewModel
        {
            Id = stop.Id,
            Name = stop.Name,
            Longitude = stop.Longitude,
            Latitude = stop.Latitude,
            RouteId = stop.RouteId
        };
    }
}