#nullable disable

using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class CreateEntryModel
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    public int BusId { get; set; }
    public int DriverId { get; set; }
    public int LoopId { get; set; }
    public int StopId { get; set; }

    public static CreateEntryModel CreateEntry(int id)
    {
        return new CreateEntryModel
        {
            Id = id,
            Timestamp = DateTime.Now,
            Boarded = 0,
            LeftBehind = 0
        };
    }
}
