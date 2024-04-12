using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class AddRouteToLoopModel
{
    [Required]
    [Display(Name = "Loop Id")]
    public int LoopId { get; set; }
    [Required]
    [Display(Name = "Route Id")]
    public int RouteId { get; set; }

    public static AddRouteToLoopModel FromId(int Id)
    {
        return new AddRouteToLoopModel
        {
            LoopId = Id,
            RouteId = -1
        };
    }
}
