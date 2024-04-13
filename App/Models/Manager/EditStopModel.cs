#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class EditStopModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Longitude { get; set; }
    [Required]
    public double Latitude { get; set; }

    public static EditStopModel FromStop(Stop stop)
    {
        return new EditStopModel
        {
            Id = stop.Id,
            Name = stop.Name,
            Longitude = stop.Longitude,
            Latitude = stop.Latitude
        };
    }
}
