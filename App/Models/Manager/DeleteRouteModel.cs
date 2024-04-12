#nullable disable

namespace App.Models.Manager;

public class DeleteRouteModel
{
    public int Id { get; set; }

    public static DeleteRouteModel DeleteRoute(int id)
    {
        return new DeleteRouteModel
        {
            Id = id
        };
    }
}
