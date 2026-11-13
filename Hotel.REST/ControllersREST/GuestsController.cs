using Hotel.Common.Services;
using Hotel.Infrastructure.Models;
using Hotel.REST.Mapping;
using Hotel.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly ICrudServiceAsync<GuestModel> _service;

    public GuestsController(ICrudServiceAsync<GuestModel> service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GuestResponseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? amount)
    {
        IEnumerable<GuestModel> guests;

        if (page is null && amount is null)
        {
            guests = await _service.ReadAllAsync();
        }
        else
        {
            var p = page ?? 1;
            var a = amount ?? 10;
            if (p < 1 || a < 1 || a > 100)
                return BadRequest("page має бути >= 1, amount від 1 до 100.");

            guests = await _service.ReadAllAsync(p, a);
        }

        return Ok(guests.Select(g => g.ToResponse()));
    }

    // GET api/guests/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GuestResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GuestResponseModel>> GetById(Guid id)
    {
        var guest = await FindAsync(id);
        if (guest is null)
            return NotFound();

        return Ok(guest.ToResponse());
    }

    // POST api/guests
    [HttpPost]
    [ProducesResponseType(typeof(GuestResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GuestResponseModel>> Create([FromBody] GuestCreateModel model)
    {
        var entity = model.ToEntity();

        var created = await _service.CreateAsync(entity);
        if (!created)
            return StatusCode(StatusCodes.Status500InternalServerError, "Не вдалося створити гостя.");

        await _service.SaveAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToResponse());
    }

    // PUT api/guests/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] GuestUpdateModel model)
    {
        var entity = await FindAsync(id);
        if (entity is null)
            return NotFound();

        model.ApplyTo(entity);

        await _service.UpdateAsync(entity);
        await _service.SaveAsync();

        return NoContent();
    }

    // DELETE api/guests/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await FindAsync(id);
        if (entity is null)
            return NotFound();

        await _service.RemoveAsync(entity);
        await _service.SaveAsync();

        return NoContent();
    }

    private async Task<GuestModel?> FindAsync(Guid id)
    {
        try
        {
            return await _service.ReadAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }
}