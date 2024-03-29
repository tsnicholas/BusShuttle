using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDatabase;
using BusShuttleModel;
namespace App.Service;

public class DatabaseService
{
    private readonly BusShuttleContext _context = new BusShuttleContext();

    public DatabaseService()
    {
    }

    public List<Bus> GetAllBuses()
    {
        return _context.Buses.OrderBy(bus => bus.Id).ToList();
    }

    public void CreateBus(Bus bus)
    {
        _context.Buses.Add(bus);
        _context.SaveChanges();
    }

    public Bus GetBusById(int id)
    {
        return _context.Buses.Single(bus => bus.Id == id);
    }

    public void EditBusById(int id, int busNumber)
    {
        Bus selectedBus = GetBusById(id);
        if(selectedBus == null) return;
        selectedBus.Update(busNumber);
        _context.SaveChanges();
    }

    public void DeleteBusById(int id)
    {
        Bus selectedBus = GetBusById(id);
        if(selectedBus == null) return;
        _context.Buses.Remove(selectedBus);
        _context.SaveChanges();
    }
}
