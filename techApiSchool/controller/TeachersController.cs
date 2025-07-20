using domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services;

namespace Controllers;

/// <summary>
/// CRUD para profesores
/// </summary>

[ApiController]
[Route("api/Teachers")]
public class TeachersController : ControllerBase
{
    private readonly TeacherService _service;

    public TeachersController(TeacherService service)
    {
        _service = service;
    }

    /// <summary>
    /// Consulta general de todos los profesores
    /// </summary>
    /// <returns>Profesores</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

    /// <summary>
    /// Consulta general por id del profesor
    /// </summary>
    /// <param name="id">Id del profesor</param>
    /// <returns>Profesores con filtro by id</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var teacher = await _service.GetByIdAsync(id);
        if (teacher == null) return NotFound();
        return Ok(teacher);
    }
    /// <summary>
    /// Crear Nuevos profesores
    /// </summary>
    /// <param name="dto">Información nueva del profesor</param>
    /// <returns>Profesor</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TeacherDto dto)
    {
        try
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { Id = id });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Editar profesores por id
    /// </summary>
    /// <param name="id">Id de tipo uniqueidentifier del profesor a modificar</param>
    /// <param name="dto">Información nueva del profesor</param>
    /// <returns>Profesor</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] TeacherDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Cambiar a estado Eliminado sin eliminar permanentemente el profesor
    /// </summary>
    /// <param name="id">Id de tipo uniqueidentifier del profesor que se desea eliminar</param>
    /// <returns>Ok</returns>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
