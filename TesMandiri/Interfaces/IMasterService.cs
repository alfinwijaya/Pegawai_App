using TesMandiri.Models;

namespace TesMandiri.Interfaces;

public interface IMasterService<T>
{
    List<T> Get();
    bool Update(T obj);
}

public interface IMasterInt<T> : IMasterService<T>
{
    T GetById(int id);
    int Create(T obj);
    bool Delete(int id);
}

public interface IMasterString<T> : IMasterService<T>
{
    T GetById(string id);
    string Create(T obj);
    bool Delete(string id);
}
