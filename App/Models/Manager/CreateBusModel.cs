using BusShuttleModel;
namespace App.Models.Manager;

public class CreateBusModel
{
    public int Id { get; set; }
    public int BusNumber { get; set; }

    public static CreateBusModel CreateBus(int id)
    {
        return new CreateBusModel
        {
            Id = id,
            BusNumber = -1
        };
    }
}
