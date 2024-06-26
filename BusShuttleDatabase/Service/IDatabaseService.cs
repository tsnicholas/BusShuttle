using System.Linq.Expressions;
using BusShuttleModel;
namespace Database.Service;

public interface IDatabaseService
{
    List<T> GetAll<T>() where T : IBusData;
    T? GetById<T>(int id) where T : IBusData;
    int GenerateId<T>() where T : IBusData;
    void CreateEntity<T>(T entity) where T : IBusData;
    void UpdateById<T>(int id, T entity) where T : IBusData;
    void DeleteById<T>(int id) where T : IBusData;
    Loop GetLoopWithStopsById(int id);
    Driver GetDriverByEmail(string email);
    void SaveChanges();
}
