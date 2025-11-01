using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Consultorio.Data;
using Consultorio.Models;

namespace Consultorio.Controllers;

[ApiController]
[Route("api/v1/[controller]")] // Utiliza nome  do arquivo/classe
public class PatientsController : ControllerBase{
    private readonly AppDbContext _db;
    public PatientsController(AppDbContext db) =>_db = db;

    //GET /api/v1/patients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        => Ok(await _db.Patients.OrderBy(p => p.Id).ToListAsync());
    
    //GET /api/v1/patients/1(id)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Patient>> GetById(int id)
        => await _db.Patients.FindAsync(id) is { } p ? Ok(p) : NotFound();

    //POST /api/v1/patients
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Patient p){
        if(!string.IsNullOrWhiteSpace(p.Email) &&
            await _db.Patients.AnyAsync(x => x.Email == p.Email)){
                return Conflict(new {error = "Email já cadastrado"});
            }

        if (!string.IsNullOrWhiteSpace(p.CPF) &&
            await _db.Patients.AnyAsync(x => x.CPF == p.CPF)){
                return Conflict(new { error = "CPF já cadastrado" });
            }

        _db.Patients.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof (GetById), new {id = p.Id}, p);
    }

    //PUT /api/v1/patients/1(id)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Patient p){
        p.Id = id;

         if(!string.IsNullOrWhiteSpace(p.Email) &&
            await _db.Patients.AnyAsync(x => x.Email == p.Email && x.Id != id)){
                return Conflict(new {error = "Email já cadastrado."});
            }

        if (!string.IsNullOrWhiteSpace(p.CPF) &&
            await _db.Patients.AnyAsync(x => x.CPF == p.CPF && x.Id != id)){
            return Conflict(new { error = "CPF já cadastrado" });
            }

        if(!await _db.Patients.AnyAsync(x=> x.Id == id)) return NotFound();

        _db.Entry(p).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return Ok();
    } 

    // DELETE /api/v1/patients/1(id)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete (int id){
        var p = await _db.Patients.FindAsync(id);
        if(p is null) return NotFound();

        _db.Patients.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}