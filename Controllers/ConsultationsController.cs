using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Consultorio.Data;
using Consultorio.Models;

namespace Consultorio.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ConsultationsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ConsultationsController(AppDbContext db) => _db = db;

    // GET /api/v1/consultations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Consultation>>> GetAll()
        => Ok(await _db.Consultations.OrderBy(c => c.Id).ToListAsync());

    // GET /api/v1/consultations/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Consultation>> GetById(int id)
        => await _db.Consultations.FindAsync(id) is { } c ? Ok(c) : NotFound();

    // POST /api/v1/consultations
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Consultation c)
    {
        // Verifica se o paciente existe
        if (!await _db.Patients.AnyAsync(p => p.Id == c.PatientId))
            return UnprocessableEntity(new { error = "Paciente não encontrado." });

        _db.Consultations.Add(c);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
    }

    // PUT /api/v1/consultations/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Consultation c)
    {
        c.Id = id;

        if (!await _db.Consultations.AnyAsync(x => x.Id == id))
            return NotFound();

        // Verifica se o paciente existe
        if (!await _db.Patients.AnyAsync(p => p.Id == c.PatientId))
            return UnprocessableEntity(new { error = "Paciente não encontrado." });

        _db.Entry(c).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Ok();
    }

    // DELETE /api/v1/consultations/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Consultations.FindAsync(id);
        if (c is null) return NotFound();

        _db.Consultations.Remove(c);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
