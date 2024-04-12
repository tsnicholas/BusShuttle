#nullable disable

namespace App.Models.Manager;

public class DeleteLoopModel
{
    public int Id { get; set; }

    public static DeleteLoopModel DeleteLoop(int id)
    {
        return new DeleteLoopModel
        {
            Id = id
        };
    }
}
