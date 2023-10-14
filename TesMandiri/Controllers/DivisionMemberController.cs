using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;
using TesMandiri.Services;

namespace TesMandiri.Controllers;

[ApiController]
[Route("transaction/division_member")]
public class DivisionMemberController : ControllerBase
{
    private readonly IMasterInt<EmployeeBase> _employeeService;
    private readonly IMasterString<DivisionBase> _divisionService;
    private readonly IDivisionMemberService _divisionMemberService;
    public DivisionMemberController(IMasterInt<EmployeeBase> employeeService, IMasterString<DivisionBase> divisionService, IDivisionMemberService divisionMemberService)
    {
        _employeeService = employeeService;
        _divisionService = divisionService;
        _divisionMemberService = divisionMemberService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<Division> divMembers = _divisionMemberService.Get();
            return Ok(divMembers);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("{code}")]
    public IActionResult GetById(string code)
    {
        try
        {
            var division = _divisionService.GetById(code);
            if (string.IsNullOrWhiteSpace(division.DivisionCode))
                return NotFound($"Division doesn't exist");

            var divMembers = _divisionMemberService.GetById(code);
            if (string.IsNullOrWhiteSpace(divMembers!.DivisionCode))
                return NotFound($"Division doesn't have any member yet");

            return Ok(divMembers);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody] DivisionMemberDto division)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(division.DivisionCode))
                return BadRequest($"Division code must not be empty");

            if (division.Employee.Count < 0)
                return BadRequest($"Division member cannot be empty");

            if (division.Employee.Any(e => e.EmployeeId == 0))
                return BadRequest($"Employee id must be greater than 0");

            var check = _divisionService.GetById(division.DivisionCode);
            if (string.IsNullOrWhiteSpace(check.DivisionCode))
                return NotFound($"Division doesn't exist");

            var exist = _divisionMemberService.GetById(division.DivisionCode);
            if (!string.IsNullOrWhiteSpace(exist!.DivisionCode))
                return BadRequest($"Division member has been created before. To add new member please use update function");

            foreach (var employee in division.Employee)
            {
                var emp = _employeeService.GetById(employee.EmployeeId);
                if (emp.EmployeeId == 0)
                    return NotFound($"Employee with id {employee.EmployeeId} not found");

                if (_divisionMemberService.CheckEmpExist(employee.EmployeeId))
                    return NotFound($"Employee with id {employee.EmployeeId} already belong to other division");
            }

            _divisionMemberService.Create(division);

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{code}")]
    public IActionResult Update(string code, DivisionMemberDto division)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest($"Division code must be greater than 0");

            if (division.Employee.Count < 0)
                return BadRequest($"Division member cannot be empty");

            if (division.Employee.Any(e => e.EmployeeId == 0))
                return BadRequest($"Employee id must be greater than 0");

            var check = _divisionService.GetById(code);
            if (string.IsNullOrWhiteSpace(check.DivisionCode))
                return NotFound($"Division doesn't exist");

            foreach (var employee in division.Employee)
            {
                var emp = _employeeService.GetById(employee.EmployeeId);
                if (emp.EmployeeId == 0)
                    return NotFound($"Employee with id {employee.EmployeeId} not found");

                if (_divisionMemberService.CheckEmpExist(employee.EmployeeId))
                    return NotFound($"Employee with id {employee.EmployeeId} already belong to other division");
            }

            division.DivisionCode = code;
            _divisionMemberService.Update(division);

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("{code}")]
    public IActionResult Delete(string code)
    {
        try
        {
            var check = _divisionService.GetById(code);
            if (string.IsNullOrWhiteSpace(check.DivisionCode))
                return NotFound($"Division doesn't exist");

            check = _divisionMemberService.GetById(code);
            if (check is null)
                return NotFound($"Division doesn't have any registered member");

            _divisionMemberService.Delete(code);
            return NoContent();
        }
        catch
        {
            throw;
        }
    }

}
