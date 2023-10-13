using TesMandiri.Models;

namespace TesMandiri.Interfaces;

public interface IEmployeeService
{
    List<EmployeeBase> GetEmployee();

    EmployeeBase GetEmployeeById(int id);

    int CreateEmployee(string name);

    bool UpdateEmployee(EmployeeBase employee);

    bool DeleteEmployee(int id);
}
