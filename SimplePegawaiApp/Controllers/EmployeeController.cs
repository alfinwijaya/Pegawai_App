using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Controllers;

[ApiController]
[Route("master/employee")]
public class EmployeeController : ControllerBase
{
    private readonly IMasterInt<EmployeeBase> _employeeService;
    public EmployeeController(IMasterInt<EmployeeBase> employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<EmployeeBase> employees = _employeeService.Get();
            return Ok(employees);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var employee = _employeeService.GetById(id);
            if (employee.EmployeeId == 0)
                return NotFound($"Employee with id {id} doesn't exist");

            return Ok(employee);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody]EmployeeBase employee)
    {
        try
        {
            var id = _employeeService.Create(employee);

            EmployeeBase created = new() { EmployeeId = id, EmployeeName = employee.EmployeeName };
            return CreatedAtAction(nameof(GetById), new { id = id}, created);
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, EmployeeBase employee)
    {
        try
        {
            var check = _employeeService.GetById(id);
            if (check.EmployeeId == 0)
                return NotFound($"Employee with id {id} doesn't exist");

            employee.EmployeeId = check.EmployeeId;
            _employeeService.Update(employee);

            return NoContent();
        }
        catch 
        { 
            throw; 
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var check = _employeeService.GetById(id);
            if (check.EmployeeId == 0)
                return NotFound($"Employee with id {id} doesn't exist");

            _employeeService.Delete(id);
            return NoContent();
        }
        catch
        {
            throw;
        }
    }
}
