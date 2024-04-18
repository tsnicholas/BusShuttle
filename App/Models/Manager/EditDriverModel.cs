#nullable disable

using System.ComponentModel.DataAnnotations;
using static BusShuttleModel.Driver;
namespace App.Models.Manager;

public class EditDriverModel
{
    public int Id { get; set; }
    
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Please enter the first name.")]
    public string FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Please enter the last name.")]
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }

    public static EditDriverModel FromDriver(BusShuttleModel.Driver driver)
    {
        return new EditDriverModel
        {
            Id = driver.Id,
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Email = driver.Email
        };
    }
}
