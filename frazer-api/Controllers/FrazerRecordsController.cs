using frazer_api.Models;
using frazer_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace frazer_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FrazerRecordsController : ControllerBase
{
    private readonly IFrazerRepository _repository;

    public FrazerRecordsController(IFrazerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FrazerRecord>>> GetAsync(CancellationToken cancellationToken)
    {
        var records = await _repository.GetAllAsync(cancellationToken);
        return Ok(records);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FrazerRecord>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var record = await _repository.GetByIdAsync(id, cancellationToken);
        if (record is null)
        {
            return NotFound();
        }

        return Ok(record);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FrazerRecord>> CreateAsync([FromBody] FrazerRecordCreateDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var created = await _repository.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FrazerRecord>> UpdateAsync(int id, [FromBody] FrazerRecordUpdateDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated = await _repository.UpdateAsync(id, request, cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
