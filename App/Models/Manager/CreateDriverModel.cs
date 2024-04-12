using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class CreateDriverModel
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Please enter your first name.")]
    public string FirstName { get; set; }
    
    [Required]
    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Please enter your last name.")]
    public string LastName { get; set; }
    
    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Password")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password {get; set; }

    public static CreateDriverModel CreateDriver(int id)
    {
        return new CreateDriverModel
        {
            Id = id,
            FirstName = "",
            LastName = "",
            Email = "",
            Password = ""
        };
    }
}
