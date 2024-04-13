#nullable disable

using BusShuttleModel;
namespace App.Models.Manager;

public class RouteViewModel
{
    public int Id { get; set; }
    public int Order { get; set; }
    public int StopId { get; set; }

    public static RouteViewModel FromRoute(BusRoute route)
    {
        return new RouteViewModel
        {
            Id = route.Id,
            Order = route.Order,
            StopId = route.StopId
        };
    }
}
