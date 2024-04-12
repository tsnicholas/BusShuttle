#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class EditBusModel
{
    [Required]
    public int Id { get; set; }
    [Required]
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
