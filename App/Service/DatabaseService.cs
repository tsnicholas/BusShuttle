using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusShuttleDatabase;
using BusShuttleModel;
namespace App.Service;

public class DatabaseService : IDatabaseService
{
    private readonly IBusShuttleContext _context;

    public DatabaseService(IBusShuttleContext context)
    {
        _context = context;
    }

    public List<T> GetAll<T>(params string[] childrenToInclude) where T : IBusData
    {
        IQueryable<T> queryable = _context.Set<T>().AsQueryable();
        foreach(string child in childrenToInclude ??= [])
        {
            queryable = queryable.Include(child);
        }
        return queryable.OrderBy(entity => entity.Id).ToList();
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

    public void AddRouteToLoop(Loop loop, BusRoute route)
    {
        loop.Routes.Add(route);
        _context.SaveChanges();
    }

    public void RemoveRouteFromLoop(Loop loop, BusRoute route)
    {
        loop.Routes.Remove(route);
        _context.SaveChanges();
    }

    public Driver GetDriverByEmail(string email)
    {
        return _context.Drivers.Single(driver => driver.Email == email);
    }
}
