using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models;

public class CreateRouteModel
{
    public int Id { get; set; }
    [Required]
    public int Order { get; set; }

    public static CreateRouteModel CreateRoute(int id)
    {
        return new CreateRouteModel
        {
            Id = id
        };
    }
}
