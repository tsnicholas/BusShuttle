#nullable disable

using BusShuttleModel;
namespace App.Models.Manager;

public class EditEntryModel
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    public int BusId { get; set; }
    public int DriverId { get; set; }
    public int LoopId { get; set; }
    public int StopId { get; set; }

    public static EditEntryModel FromEntry(Entry entry)
    {
        return new EditEntryModel
        {
            Id = entry.Id,
            Timestamp = entry.Timestamp,
            Boarded = entry.Boarded,
            LeftBehind = entry.LeftBehind,
            BusId = entry.BusId ?? -1,
            DriverId = entry.DriverId ?? -1,
            LoopId = entry.LoopId ?? -1,
            StopId = entry.StopId ?? -1
        };
    }
}
