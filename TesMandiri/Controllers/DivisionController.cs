using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Controllers;

[ApiController]
[Route("master/division")]
public class DivisionController : ControllerBase
{
    private readonly IMasterString<DivisionBase> _divisionService;
    public DivisionController(IMasterString<DivisionBase> divisionService)
    {
        _divisionService = divisionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<DivisionBase> cards = _divisionService.Get();
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
            var division = _divisionService.GetById(id);
            if (string.IsNullOrWhiteSpace(division.DivisionCode))
                return NotFound($"Division with code {id} doesn't exist");

            return Ok(division);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody]DivisionBase division)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(division.DivisionCode))
                return BadRequest($"Division code cannot be empty");

            var check = _divisionService.GetById(division.DivisionCode);
            if (!string.IsNullOrWhiteSpace(check.DivisionCode))
                return NotFound($"Division with code {division.DivisionCode} already exist");

            var id = _divisionService.Create(division);

            DivisionBase created = new() { DivisionCode = id, DivisionName = division.DivisionName };
            return CreatedAtAction(nameof(GetById), new { id = id}, created);
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, DivisionBase division)
    {
        try
        {
            var check = _divisionService.GetById(id);
            if (string.IsNullOrWhiteSpace(check.DivisionCode))
                return NotFound($"Division with code {id} doesn't exist");

            division.DivisionCode = check.DivisionCode;
            _divisionService.Update(division);

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
            var check = _divisionService.GetById(id);
            if (string.IsNullOrWhiteSpace(check.DivisionCode))
                return NotFound($"Division with code {id} doesn't exist");

            _divisionService.Delete(id);
            return NoContent();
        }
        catch
        {
            throw;
        }
    }
}
