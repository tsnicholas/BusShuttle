using BusShuttleModel;
namespace App.Models.Manager;

public class EditRoutesInLoopModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<BusShuttleModel.Route> Routes { get; set; }

    public static EditRoutesInLoopModel FromLoop(Loop loop)
    {
        return new EditRoutesInLoopModel
        {
            Id = loop.Id,
            Name = loop.Name,
            Routes = loop.Routes
        };
    }
}
