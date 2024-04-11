using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class CreateStopModel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int RouteId { get; set; }

    public static CreateStopModel CreateStop(int id)
    {
        return new CreateStopModel
        {
            Id = id,
            Name = "",
            Longitude = 0.0,
            Latitude = 0.0,
            RouteId = -1
        };
    }
}
