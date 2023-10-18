using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Controllers;

[ApiController]
[Route("master/task")]
public class TaskController : ControllerBase
{
    private readonly IMasterString<TaskBase> _taskService;
    public TaskController(IMasterString<TaskBase> taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<TaskBase> cards = _taskService.Get();
            return Ok(cards);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        try
        {
            var task = _taskService.GetById(id);
            if (string.IsNullOrWhiteSpace(task.TaskCode))
                return NotFound($"Task with code {id} doesn't exist");

            return Ok(task);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody]TaskBase task)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(task.TaskCode))
                return BadRequest($"Task code cannot be empty");

            var check = _taskService.GetById(task.TaskCode);
            if (!string.IsNullOrWhiteSpace(check.TaskCode))
                return NotFound($"Task with code {task.TaskCode} already exist");

            var id = _taskService.Create(task);

            TaskBase created = new() { TaskCode = id, TaskName = task.TaskName };
            return CreatedAtAction(nameof(GetById), new { id = id}, created);
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, TaskBase task)
    {
        try
        {
            var check = _taskService.GetById(id);
            if (string.IsNullOrWhiteSpace(check.TaskCode))
                return NotFound($"Task with code {id} doesn't exist");

            task.TaskCode = check.TaskCode;
            _taskService.Update(task);

            return NoContent();
        }
        catch 
        { 
            throw; 
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        try
        {
            var check = _taskService.GetById(id);
            if (string.IsNullOrWhiteSpace(check.TaskCode))
                return NotFound($"Task with code {id} doesn't exist");

            _taskService.Delete(id);
            return NoContent();
        }
        catch
        {
            throw;
        }
    }
}
