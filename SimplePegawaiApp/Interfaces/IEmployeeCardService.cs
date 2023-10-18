using TesMandiri.Models;

namespace TesMandiri.Interfaces;

public interface IEmployeeCardService
{
    List<EmployeeCard> Get();
    EmployeeCard? GetById(int id);
    int Create(EmployeeCardDto employeeCard);
    bool Update(EmployeeCardDto employeeCard);
    bool Delete(int id);

}
