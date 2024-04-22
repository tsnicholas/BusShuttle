#nullable disable

using System.ComponentModel.DataAnnotations;
namespace App.Models.Manager;

public class CreateBusModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Bus Number")]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a positive integer.")]
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
