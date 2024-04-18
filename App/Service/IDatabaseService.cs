using BusShuttleDatabase;
using BusShuttleModel;
namespace App.Service;

public interface IDatabaseService
{
    List<T> GetAll<T>(params string[] childrenToInclude) where T : IBusData;
    T GetById<T>(int id, params string[] childrenToInclude) where T : IBusData;
    void CreateEntity<T>(T entity) where T : IBusData;
    void UpdateById<T>(int id, T entity) where T : IBusData;
    void DeleteById<T>(int id) where T : IBusData;
    
    void AddRouteToLoop(Loop loop, BusRoute route);
    Loop GetLoopWithStopsById(int id);
    Driver GetDriverByEmail(string email);
}
