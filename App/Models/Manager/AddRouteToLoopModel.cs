#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class AddRouteToLoopModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Loop Id.")]
    public int LoopId { get; set; }
    [Required]
    public List<BusRoute> Routes { get; set; } = new();
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Route Id.")]
    public int RouteId { get; set; } = 1;

    public static AddRouteToLoopModel FromId(int Id, List<BusRoute> routes)
    {
        return new AddRouteToLoopModel
        {
            LoopId = Id,
            Routes = routes
        };
    }
}
