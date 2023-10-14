using Microsoft.AspNetCore.Mvc;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Controllers;

[ApiController]
[Route("master/card")]
public class IdCardController : ControllerBase
{
    private readonly IMasterInt<IdCardBase> _cardService;
    public IdCardController(IMasterInt<IdCardBase> cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<IdCardBase> cards = _cardService.Get();
            return Ok(cards);
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
            var card = _cardService.GetById(id);
            if (card.CardNumber == 0)
                return NotFound($"Card with number {id} doesn't exist");

            return Ok(card);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody]IdCardBase card)
    {
        try
        {
            if (card.CardNumber == 0)
                return BadRequest($"Card number must be greater than zero");

            var check = _cardService.GetById(card.CardNumber);
            if (check.CardNumber > 0)
                return NotFound($"Card with number {card.CardNumber} already exist");

            var id = _cardService.Create(card);

            IdCardBase created = new() { CardNumber = id, CardDescription = card.CardDescription };
            return CreatedAtAction(nameof(GetById), new { id = id}, created);
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, IdCardBase card)
    {
        try
        {
            var check = _cardService.GetById(id);
            if (check.CardNumber == 0)
                return NotFound($"Card with number {id} doesn't exist");

            card.CardNumber = check.CardNumber;
            _cardService.Update(card);

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
            var check = _cardService.GetById(id);
            if (check.CardNumber == 0)
                return NotFound($"Card with number {id} doesn't exist");

            _cardService.Delete(id);
            return NoContent();
        }
        catch
        {
            throw;
        }
    }
}
