using Microsoft.EntityFrameworkCore;
using BusShuttleModel;
namespace Database.Service;

public class DatabaseService(BusShuttleContext context) : IDatabaseService
{
    private readonly BusShuttleContext _context = context;

    public List<T> GetAll<T>(params string[] childrenToInclude) where T : IBusData
    {
        IQueryable<T> queryable = _context.Set<T>().AsQueryable();
        foreach(string child in childrenToInclude ??= [])
        {
            queryable = queryable.Include(child);
        }
        return [.. queryable.OrderBy(entity => entity.Id)];
    }

    public T GetById<T>(int id, params string[] childrenToInclude) where T : IBusData
    {
        IQueryable<T> queryable = _context.Set<T>().AsQueryable();
        foreach(string child in childrenToInclude ??= [])
        {
            queryable = queryable.Include(child);
        }
        return queryable.Single(entity => entity.Id == id);
    }

    public int GenerateId<T>() where T : IBusData
    {
        Random randomNumber = new(Guid.NewGuid().GetHashCode());
        int nextId;
        do {
            nextId = randomNumber.Next(1, 1000000);
        } while(IsIdTaken<T>(nextId));
        return nextId;
    }

    private bool IsIdTaken<T>(int id) where T : IBusData
    {
        try 
        {
            return _context.Set<T>().Single(entity => entity.Id == id) != null;
        }
        catch (InvalidOperationException exception)
        {
            if(exception.Message == "Sequence contains no elements")
            {
                return false;
            }
            throw exception ?? throw new InvalidOperationException();
        }
    }

    public void CreateEntity<T>(T entity) where T : IBusData
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }

    public void UpdateById<T>(int id, T entity) where T : IBusData
    {
        T selectedEntity = GetById<T>(id);
        if(selectedEntity == null) {
            return;
        }
        selectedEntity.Update(entity);
        _context.SaveChanges();
    }

    public void DeleteById<T>(int id) where T : IBusData
    {
        T selectedEntity = GetById<T>(id);
        if(selectedEntity == null) {
            return;
        }
        _context.Set<T>().Remove(selectedEntity);
        _context.SaveChanges();
    }

    public Loop GetLoopWithStopsById(int id)
    {
        return _context.Loops.Include(loop => loop.Entries).Include(loop => loop.Routes)
            .ThenInclude(route => route.Stop).Single(loop => loop.Id == id);
    }

    public Driver GetDriverByEmail(string email)
    {
        return _context.Drivers.Single(driver => driver.Email == email);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
