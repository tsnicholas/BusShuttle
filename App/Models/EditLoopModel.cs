using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models;

public class EditLoopModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static EditLoopModel FromLoop(Loop loop)
    {
        return new EditLoopModel
        {
            Id = loop.Id,
            Name = loop.Name
        };
    }
}
