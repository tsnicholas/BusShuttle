namespace App.Models;

public class DeleteStopModel
{
    public int Id { get; set; }

    public static DeleteStopModel DeleteStop(int id)
    {
        return new DeleteStopModel
        {
            Id = id
        };
    }
}
