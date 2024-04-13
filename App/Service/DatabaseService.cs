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
        if(id <= 0) {
            throw new Exception("Bus Id must be greater than zero.");
        }
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

    public List<Driver> GetAllDrivers()
    {
        return _context.Drivers.OrderBy(driver => driver.Id).ToList();
    }

    public void CreateDriver(Driver driver)
    {
        _context.Drivers.Add(driver);
        _context.SaveChanges();
    }

    public Driver GetDriverById(int id)
    {
        return _context.Drivers.Single(driver => driver.Id == id);
    }

    public Driver GetDriverByEmail(string email)
    {
        return _context.Drivers.Single(driver => driver.Email == email);
    }

    public void EditDriverById(int id, string firstName, string lastName)
    {
        Driver selectedDriver = GetDriverById(id);
        if(selectedDriver == null) return;
        selectedDriver.Update(firstName, lastName);
        _context.SaveChanges();
    }

    public void DeleteDriverById(int id)
    {
        Driver selectedDriver = GetDriverById(id);
        if(selectedDriver == null) return;
        _context.Drivers.Remove(selectedDriver);
        _context.SaveChanges();
    }

    public List<BusShuttleModel.Route> GetAllRoutes()
    {
        return _context.Routes.OrderBy(route => route.Id).ToList();
    }

    public void CreateRoute(BusShuttleModel.Route route)
    {
        _context.Routes.Add(route);
        _context.SaveChanges();
    }

    public BusShuttleModel.Route GetRouteById(int id)
    {
        return _context.Routes.Single(route => route.Id == id);
    }

    public void EditRouteById(int id, int order)
    {
        BusShuttleModel.Route selectedRoute = GetRouteById(id);
        if(selectedRoute == null) return;
        selectedRoute.Update(order);
        _context.SaveChanges();
    }

    public void DeleteRouteById(int id)
    {
        BusShuttleModel.Route selectedRoute = GetRouteById(id);
        if(selectedRoute == null) return;
        _context.Routes.Remove(selectedRoute);
        _context.SaveChanges();
    }

    public List<Stop> GetAllStops()
    {
        return _context.Stops.OrderBy(stop => stop.Id).ToList();
    }

    public void CreateStop(Stop stop)
    {
        _context.Stops.Add(stop);
        _context.SaveChanges();
    }

    public Stop GetStopById(int id)
    {
        return _context.Stops.Single(stop => stop.Id == id);
    }

    public void EditStopById(int id, string name, double latitude, double longitude, int routeId)
    {
        Stop selectedStop = GetStopById(id);
        if(selectedStop == null) return;
        selectedStop.Update(name, latitude, longitude, routeId);
        _context.SaveChanges();
    }

    public void DeleteStopById(int id)
    {
        Stop selectedStop = GetStopById(id);
        if(selectedStop == null) return;
        _context.Stops.Remove(selectedStop);
        _context.SaveChanges();
    }

    public List<Entry> GetAllEntries()
    {
        return _context.Entries.OrderBy(entry => entry.Id).ToList();
    }

    public void CreateEntry(Entry entry)
    {
        _context.Entries.Add(entry);
        _context.SaveChanges();
    }

    public Entry GetEntryWithId(int id)
    {
        return _context.Entries.Single(entry => entry.Id == id); 
    }

    public void EditEntryWithId(int id, DateTime timestamp, int boarded, int leftBehind, int busId, int driverId, int loopId, int stopId)
    {
        Entry selectedEntry = GetEntryWithId(id);
        if(selectedEntry == null) return;
        selectedEntry.Update(timestamp, boarded, leftBehind, busId, driverId, loopId, stopId);
        _context.SaveChanges();
    }

    public void DeleteEntryWithId(int id)
    {
        Entry selectedEntry = GetEntryWithId(id);
        if(selectedEntry == null) return;
        _context.Entries.Remove(selectedEntry);
        _context.SaveChanges();
    }

    public List<Loop> GetAllLoops()
    {
        return _context.Loops.OrderBy(loop => loop.Id).ToList();
    }

    public void CreateLoop(Loop loop)
    {
        _context.Loops.Add(loop);
        _context.SaveChanges();
    }

    public Loop GetLoopWithId(int id)
    {
        if(id <= 0) {
            throw new Exception("Loop Id must be greater than zero.");
        }
        return _context.Loops.Single(loop => loop.Id == id);
    }

    public void EditLoopWithId(int id, string name)
    {
        Loop selectedLoop = GetLoopWithId(id);
        selectedLoop.Update(name);
        _context.SaveChanges();
    }

    public void DeleteLoopWithId(int id)
    {
        Loop selectedLoop = GetLoopWithId(id);
        _context.Loops.Remove(selectedLoop);
        _context.SaveChanges();
    }

    public void AddRouteToLoop(Loop loop, BusShuttleModel.Route route)
    {
        loop.Routes.Add(route);
        _context.SaveChanges();
    }

    public void RemoveRouteFromLoop(Loop loop, BusShuttleModel.Route route)
    {
        loop.Routes.Remove(route);
        _context.SaveChanges();
    }
}
