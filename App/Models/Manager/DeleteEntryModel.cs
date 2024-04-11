namespace App.Models.Manager;

public class DeleteEntryModel
{
    public int Id { get; set; }

    public static DeleteEntryModel DeleteEntry(int id)
    {
        return new DeleteEntryModel
        {
            Id = id
        };
    }
}
