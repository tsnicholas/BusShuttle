namespace App.Models;

public class DeleteBusModel
{
    public int Id { get; set; }

    public static DeleteBusModel DeleteBus(int id)
    {
        return new DeleteBusModel
        {
            Id = id
        };
    }
}
