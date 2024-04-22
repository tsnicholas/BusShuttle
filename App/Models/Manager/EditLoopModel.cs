#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class EditLoopModel
{
    [Required]
    public int Id { get; set; }
    [Required]
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
