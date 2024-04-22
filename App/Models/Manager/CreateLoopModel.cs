#nullable disable

using System.ComponentModel.DataAnnotations;
namespace App.Models.Manager;

public class CreateLoopModel
{
    [Required]
    public int Id { get; set; }
    [Required]
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
