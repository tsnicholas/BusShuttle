#nullable disable

using BusShuttleModel;
namespace App.Models.Manager;

public class EditRouteModel
{
    public int Id { get; set; }
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
