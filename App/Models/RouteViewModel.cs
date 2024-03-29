using BusShuttleModel;
namespace App.Models;

public class RouteViewModel
{
    public int Id { get; set; }
    public int Order { get; set; }

    public static RouteViewModel FromRoute(BusShuttleModel.Route route)
    {
        return new RouteViewModel
        {
            Id = route.Id,
            Order = route.Order
        };
    }
}
