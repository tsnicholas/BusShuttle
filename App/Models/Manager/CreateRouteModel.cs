#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class CreateRouteModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int Order { get; set; }

    public static CreateRouteModel CreateRoute(int id)
    {
        return new CreateRouteModel
        {
            Id = id,
            Order = -1
        };
    }
}
