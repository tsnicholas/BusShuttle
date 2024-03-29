using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models;

public class CreateDriverModel
{
    public int Id { get; set; }
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; }
    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; }

    public static CreateDriverModel CreateDriver(int id)
    {
        return new CreateDriverModel
        {
            Id = id,
            FirstName = "",
            LastName = ""
        };
    }
}
