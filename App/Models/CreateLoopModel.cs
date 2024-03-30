using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models;

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
