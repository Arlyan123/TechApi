using domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services;

namespace Controllers;

/// <summary>
/// CRUD para Cursos
/// </summary>


[ApiController]
[Route("api/Subjects")]
public class SubjectController : ControllerBase
{
    private readonly SubjectsService _service;

    public SubjectController(SubjectsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Consulta general de todos los cursos 
    /// </summary>
    /// <returns>Cursos</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subjects = await _service.GetAllAsync();
        return Ok(subjects);
    }

    /// <summary>
    /// Consulta general por id del curso
    /// </summary>
    /// <param name="id">Id del curso</param>
    /// <returns>Curso con filtro by id</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var subject = await _service.GetByIdAsync(id);
        if (subject == null) return NotFound();
        return Ok(subject);
    }
    /// <summary>
    /// Crear Nuevos cursos
    /// </summary>
    /// <param name="dto">Información nueva del curso. EJ: Name: Frances y Id de Profesor</param>
    /// <returns>Cursos</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SubjectDto dto)
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
    /// Editar cursos por id
    /// </summary>
    /// <param name="id">Id de tipo uniqueidentifier del curso a modificar</param>
    /// <param name="dto">Información nueva del curso</param>
    /// <returns>Cursos</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SubjectDto dto)
    {
        await _service.UpdateAsync(id, dto);
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

