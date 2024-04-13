#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class CreateRouteModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int Order { get; set; }
    [Required]
    public List<Stop> Stops { get; set; } = new();
    [Required]
    public int StopId { get; set; } = 1;

    public static CreateRouteModel CreateRoute(int id, List<Stop> stops)
    {
        return new CreateRouteModel
        {
            Id = id,
            Order = -1,
            Stops = stops
        };
    }
}
