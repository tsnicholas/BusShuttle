#nullable disable

namespace App.Models.Manager;

public class DriverViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public static DriverViewModel FromDriver(BusShuttleModel.Driver driver)
    {
        return new DriverViewModel
        {
            Id = driver.Id,
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Email = driver.Email
        };
    }
}
