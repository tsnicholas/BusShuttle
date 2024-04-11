namespace App.Models.Manager;

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
