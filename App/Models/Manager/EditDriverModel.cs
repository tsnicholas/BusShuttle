using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class EditDriverModel
{
    public int Id { get; set; }
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; }
    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; }

    public static EditDriverModel FromDriver(Driver driver)
    {
        return new EditDriverModel
        {
            Id = driver.Id,
            FirstName = driver.FirstName,
            LastName = driver.LastName
        };
    }
}
