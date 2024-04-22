#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class BusViewModel
{
    public int Id { get; set; }
    [Display(Name = "Bus Number")]
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
