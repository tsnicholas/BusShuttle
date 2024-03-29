using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models;

public class EditRouteModel
{
    public int Id { get; set; }
    [Required]
    public int Order { get; set; }

    public static EditRouteModel FromRoute(BusShuttleModel.Route route)
    {
        return new EditRouteModel
        {
            Id = route.Id,
            Order = route.Order
        };
    }
}
