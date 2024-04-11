using System.ComponentModel.DataAnnotations;
using BusShuttleModel;
namespace App.Models.Manager;

public class EntryViewModel
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    public int BusId { get; set; }
    public int DriverId { get; set; }
    public int LoopId { get; set; }
    public int StopId { get; set; }

    public static EntryViewModel FromEntry(Entry entry)
    {
        return new EntryViewModel
        {
            Id = entry.Id,
            Timestamp = entry.Timestamp,
            Boarded = entry.Boarded,
            LeftBehind = entry.LeftBehind,
            BusId = entry.BusId,
            DriverId = entry.DriverId,
            LoopId = entry.LoopId,
            StopId = entry.StopId
        };
    }
}
