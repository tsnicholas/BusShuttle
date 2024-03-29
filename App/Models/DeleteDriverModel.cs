namespace App.Models;

public class DeleteDriverModel
{
    public int Id { get; set; }

    public static DeleteDriverModel DeleteDriver(int id)
    {
        return new DeleteDriverModel
        {
            Id = id
        };
    }
}
