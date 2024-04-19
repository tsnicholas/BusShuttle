using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
namespace Database.Test.Service;

public class DatabaseFixture : IDisposable
{
    private static readonly object _lock = new();
    private static bool _databaseInitialized;
    public BusShuttleContext? Context { get; private set; }

    public DatabaseFixture()
    {
        lock (_lock)
        {
            if(!_databaseInitialized)
            {
                Context = CreateContext();
                Context.Database.OpenConnection();
                Context.Database.EnsureCreated();
                _databaseInitialized = true;
            }
        }
    }

    private static BusShuttleContext CreateContext()
    {
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder{DataSource=":memory:"}.ToString());
        return new BusShuttleContext(new DbContextOptionsBuilder<BusShuttleContext>().UseSqlite(connection).Options);
    }

    public void Dispose()
    {
        if(Context == null) return;
        GC.SuppressFinalize(this);
        Context.Dispose();
    }
}
