using TesMandiri.Models;

namespace TesMandiri.Interfaces;

public interface IEmployeeTaskService
{
    List<EmployeeTask> Get();
    EmployeeTask? GetById(int id);
    bool Create(EmployeeTaskDto employee);
    bool Update(EmployeeTaskDto employee);
    bool Delete(int id);
}
