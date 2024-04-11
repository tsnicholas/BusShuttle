using BusShuttleModel;
namespace App.Models.Manager;

public class EditBusModel
{
    public int Id { get; set; }
    public int BusNumber { get; set; }

    public static EditBusModel FromBus(Bus bus)
    {
        return new EditBusModel
        {
            Id = bus.Id,
            BusNumber = bus.BusNumber
        };
    }
}
