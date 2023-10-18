using TesMandiri.Models;

namespace TesMandiri.Interfaces;

public interface IDivisionMemberService
{
    List<Division> Get();
    Division? GetById(string code);
    bool Create(DivisionMemberDto division);
    bool Update(DivisionMemberDto division);
    bool Delete(string code);
    bool CheckEmpExist(int id, string? code = null);
}
