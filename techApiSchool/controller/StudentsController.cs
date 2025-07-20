using domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services;

namespace Controllers;

/// <summary>
/// CRUD para Estudiantes
/// </summary>


[ApiController]
[Route("api/Students")]
public class StudentsController : ControllerBase
{
    private readonly StudentService _service;
    public StudentsController(StudentService service) => _service = service;

    /// <summary>
    /// Consulta general de todos los registros 
    /// </summary>
    /// <returns>Estudiantes</returns>
    [Authorize][HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    /// <summary>
    /// Consulta general por id del registro
    /// </summary>
    /// <param name="id">Id del registro</param>
    /// <returns>registro con filtro by id</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(id));
    /// <summary>
    /// Crear Nuevos Registros
    /// </summary>
    /// <param name="dto">Información nueva del registro</param>
    /// <returns>Estudiante</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentDto dto)
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Age = dto.Age,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow
        };

        await _service.CreateAsync(student);
        return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
    }
    /// <summary>
    /// Editar registros por id
    /// </summary>
    /// <param name="id">Id de tipo uniqueidentifier del registro a modificar</param>
    /// <param name="dto">Información nueva del registro</param>
    /// <returns>Estudiante</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto dto)
    {
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
