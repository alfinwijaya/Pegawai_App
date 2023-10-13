using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Controllers;

[ApiController]
[Route("master/employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult GetEmployees()
    {
        try
        {
            List<EmployeeBase> employees = _employeeService.GetEmployee();
            return Ok(employees);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        try
        {
            var employee = _employeeService.GetEmployeeById(id);
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
    public IActionResult CreateEmployee([FromBody]string name)
    {
        try
        {
            var id = _employeeService.CreateEmployee(name);

            EmployeeBase employee = new() { EmployeeId = id, EmployeeName = name };
            return CreatedAtAction(nameof(GetEmployeeById), new { id = id}, employee);
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEmployee(int id, EmployeeBase employee)
    {
        try
        {
            var check = _employeeService.GetEmployeeById(id);
            if (check.EmployeeId == 0)
                return NotFound($"Employee with id {id} doesn't exist");

            employee.EmployeeId = check.EmployeeId;
            _employeeService.UpdateEmployee(employee);

            return NoContent();
        }
        catch 
        { 
            throw; 
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        var check = _employeeService.GetEmployeeById(id);
        if (check.EmployeeId == 0)
            return NotFound($"Employee with id {id} doesn't exist");

        _employeeService.DeleteEmployee(id);
        return NoContent();
    }
}
