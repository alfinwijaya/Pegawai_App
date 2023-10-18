using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;
using TesMandiri.Services;

namespace TesMandiri.Controllers;

[ApiController]
[Route("transaction/employee_task")]
public class EmployeeTaskController : ControllerBase
{
    private readonly IMasterInt<EmployeeBase> _employeeService;
    private readonly IMasterString<TaskBase> _taskService;
    private readonly IEmployeeTaskService _employeeTaskService;
    public EmployeeTaskController(IMasterInt<EmployeeBase> employeeService, IMasterString<TaskBase> taskService, IEmployeeTaskService employeeTaskService)
    {
        _employeeService = employeeService;
        _taskService = taskService;
        _employeeTaskService = employeeTaskService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<EmployeeTask> empTask = _employeeTaskService.Get();
            return Ok(empTask);
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
            var emp = _employeeService.GetById(id);
            if (emp.EmployeeId == 0)
                return NotFound($"Employee doesn't exist");

            var empTask = _employeeTaskService.GetById(id);
            if (emp.EmployeeId == 0)
                return NotFound($"Employee doesn't have any task yet");

            return Ok(empTask);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody] EmployeeTaskDto empTask)
    {
        try
        {
            if (empTask.EmployeeId == 0)
                return BadRequest($"Employee id must be greater than 0");

            if (empTask.Tasks.Count < 0)
                return BadRequest($"Employee task cannot be empty");

            if (empTask.Tasks.Any(e => string.IsNullOrWhiteSpace(e.TaskCode)))
                return BadRequest($"Task code must not be empty");

            var check = _employeeService.GetById(empTask.EmployeeId);
            if (check.EmployeeId == 0)
                return NotFound($"Employee doesn't exist");

            var exist = _employeeTaskService.GetById(empTask.EmployeeId);
            if (exist!.EmployeeId > 0)
                return BadRequest($"Employee task has been created before. To add new member please use update function");

            foreach (var task in empTask.Tasks)
            {
                var emp = _taskService.GetById(task.TaskCode);
                if (string.IsNullOrWhiteSpace(emp.TaskCode))
                    return NotFound($"Task with code {task.TaskCode} not found");
            }

            _employeeTaskService.Create(empTask);

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, EmployeeTaskDto empTask)
    {
        try
        {
            if (id == 0)
                return BadRequest($"Employee id must be greater than 0");

            if (empTask.Tasks.Count < 0)
                return BadRequest($"Employee task cannot be empty");

            if (empTask.Tasks.Any(e => string.IsNullOrWhiteSpace(e.TaskCode)))
                return BadRequest($"Task code must not be empty");

            var check = _employeeService.GetById(id);
            if (check.EmployeeId == 0)
                return NotFound($"Employee doesn't exist");

            var exist = _employeeTaskService.GetById(id);
            if (exist!.EmployeeId == 0)
                return BadRequest($"Employee doesn't have any task yet. Please use create function");

            foreach (var task in empTask.Tasks)
            {
                var emp = _taskService.GetById(task.TaskCode);
                if (string.IsNullOrWhiteSpace(emp.TaskCode))
                    return NotFound($"Task with code {task.TaskCode} not found");
            }

            empTask.EmployeeId = id;
            _employeeTaskService.Update(empTask);

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
                return NotFound($"Employee doesn't exist");

            check = _employeeTaskService.GetById(id);
            if (check is null)
                return NotFound($"Employee doesn't have any registered task");

            _employeeTaskService.Delete(id);
            return NoContent();
        }
        catch
        {
            throw;
        }
    }

}
