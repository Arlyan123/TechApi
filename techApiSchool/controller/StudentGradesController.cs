using domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services;

namespace Controllers;

/// <summary>
/// CRUD para Calificaciones
/// </summary>

[ApiController]
[Route("api/StudentGrades")]
public class StudentGradesController : ControllerBase
{
    private readonly StudentGradesService _service;

    public StudentGradesController(StudentGradesService service)
    {
        _service = service;
    }
    /// <summary>
    /// Consulta general de todos los registros 
    /// </summary>
    /// <returns>Calificacioes</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subjects = await _service.GetAllAsync();
        return Ok(subjects);
    }
    /// <summary>
    /// Consulta general por id del registro
    /// </summary>
    /// <param name="id">Id del registro</param>
    /// <returns>registro con filtro by id</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var subject = await _service.GetByIdAsync(id);
        if (subject == null) return NotFound();
        return Ok(subject);
    }
    /// <summary>
    /// Crear Nuevos Registros
    /// </summary>
    /// <param name="dto">Información nueva del registro</param>
    /// <returns>Calificación</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentGradeDtoAU dto)
    {
        var StudentGrades = new StudentGrades
        {
            Id = Guid.NewGuid(),
            GradeValue = dto.GradeValue,
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId
        };

        await _service.CreateAsync(StudentGrades);
        return CreatedAtAction(nameof(GetById), new { id = StudentGrades.Id }, StudentGrades);

    }
    /// <summary>
    /// Editar registros por id
    /// </summary>
    /// <param name="id">Id de tipo uniqueidentifier del registro a modificar</param>
    /// <param name="dto">Información nueva del registro</param>
    /// <returns>Calificacion</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentGradeDtoAU dto)
    {
        if (dto == null) return BadRequest("El objeto dto es obligatorio.");
        if (id == Guid.Empty || dto.SubjectId == null || dto.StudentId == null)
            return BadRequest("Los campos obligatorios no pueden estar vacíos.");

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound();

        return Ok();
    }
    /// <summary>
    /// Cambiar a estado Eliminado sin eliminar permanentemente el registro
    /// </summary>
    /// <param name="id">Id de tipo uniqueidentifier del registro que se desea eliminar</param>
    /// <returns>Ok</returns>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}

