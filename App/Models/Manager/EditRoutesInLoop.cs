#nullable disable

using BusShuttleModel;
namespace App.Models.Manager;

public class EditRoutesInLoopModel
{
    public int LoopId { get; set; }
    public List<BusRoute> Routes { get; set; }

    public static EditRoutesInLoopModel FromLoop(int loopId, List<BusRoute> routes)
    {
        return new EditRoutesInLoopModel
        {
            LoopId = loopId,
            Routes = routes
        };
    }
}
