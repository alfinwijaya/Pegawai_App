using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;
using TesMandiri.Services;

namespace TesMandiri.Controllers;

[ApiController]
[Route("transaction/employee_card")]
public class EmployeeCardController : ControllerBase
{
    private readonly IMasterInt<EmployeeBase> _employeeService;
    private readonly IMasterInt<IdCardBase> _idCardService;
    private readonly IEmployeeCardService _employeeCardService;
    public EmployeeCardController(IMasterInt<EmployeeBase> employeeService, IMasterInt<IdCardBase> idCardService, IEmployeeCardService employeeCard)
    {
        _employeeService = employeeService;
        _idCardService = idCardService;
        _employeeCardService = employeeCard;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<EmployeeCard> empCards = _employeeCardService.Get();
            return Ok(empCards);
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
                return NotFound($"Employee doesn't exist");

            var empCards = _employeeCardService.GetById(id);
            if (empCards is null)
                return NotFound($"Employee doesn't have registered card");

            return Ok(empCards);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody] EmployeeCardDto empCard)
    {
        try
        {
            if (empCard.EmployeeId == 0)
                return BadRequest($"Employee Id must be greater than 0");

            if (empCard.CardNumber == 0)
                return BadRequest($"Card number must be greater than 0");

            var employee = _employeeService.GetById(empCard.EmployeeId);
            if (employee.EmployeeId == 0)
                return NotFound($"Employee doesn't exist");

            var card = _idCardService.GetById(empCard.CardNumber);
            if(card.CardNumber == 0)
                return NotFound($"Id Card doesn't exist");

            var empCards = _employeeCardService.GetById(empCard.EmployeeId);
            if (empCards is not null)
                return BadRequest($"Employee already has registered card");

            var id = _employeeCardService.Create(empCard);

            var created = _employeeCardService.GetById(id);

            return CreatedAtAction(nameof(GetById), new { id = id }, created);
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, EmployeeCardDto empCard)
    {
        try
        {
            if (id == 0)
                return BadRequest($"Employee Id must be greater than 0");

            if (empCard.CardNumber == 0)
                return BadRequest($"Card number must be greater than 0");

            var employee = _employeeService.GetById(id);
            if (employee.EmployeeId == 0)
                return NotFound($"Employee doesn't exist");

            var card = _idCardService.GetById(empCard.CardNumber);
            if (card.CardNumber == 0)
                return NotFound($"Id Card doesn't exist");

            empCard.EmployeeId = employee.EmployeeId;
            _employeeCardService.Update(empCard);

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
            var employee = _employeeService.GetById(id);
            if (employee.EmployeeId == 0)
                return NotFound($"Employee doesn't exist");

            var check = _employeeCardService.GetById(id);
            if (check is null)
                return NotFound($"Employee doesn't have registered card");

            _employeeCardService.Delete(id);
            return NoContent();
        }
        catch
        {
            throw;
        }
    }

}
