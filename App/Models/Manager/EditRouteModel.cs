#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class EditRouteModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int Order { get; set; }

    public static EditRouteModel FromRoute(BusRoute route)
    {
        return new EditRouteModel
        {
            Id = route.Id,
            Order = route.Order
        };
    }
}
