#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class LoopViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<BusShuttleModel.Route> Routes { get; set; }

    public static LoopViewModel FromLoop(Loop loop)
    {
        return new LoopViewModel
        {
            Id = loop.Id,
            Name = loop.Name,
            Routes = loop.Routes
        };
    }
}
