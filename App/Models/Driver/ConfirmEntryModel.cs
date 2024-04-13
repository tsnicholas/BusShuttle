#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Driver;

public class ConfirmEntryModel
{
    [Required]
    public BusShuttleModel.Driver BusDriver { get; set; }
    [Required]
    public Loop BusLoop { get; set; }
    [Required]
    public Bus TheBus { get; set; }
    [Required]
    public Stop BusStop { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "The number boarded can't be negative.")]
    public int Boarded {get; set; } = 0;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "The number left behind can't be negative.")]
    public int LeftBehind { get; set; } = 0;
}
