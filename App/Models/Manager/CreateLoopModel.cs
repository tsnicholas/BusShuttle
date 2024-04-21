#nullable disable

namespace App.Models.Manager;

public class CreateLoopModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static CreateLoopModel CreateLoop(int id)
    {
        return new CreateLoopModel
        {
            Id = id,
            Name = "",
        };
    }
}
