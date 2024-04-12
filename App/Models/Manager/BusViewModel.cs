#nullable disable

using BusShuttleModel;
namespace App.Models.Manager;

public class BusViewModel
{
    public int Id { get; set; }
    public int BusNumber { get; set; }

    public static BusViewModel FromBus(Bus bus)
    {
        return new BusViewModel
        {
            Id = bus.Id,
            BusNumber = bus.BusNumber
        };
    }
}
