using domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services;

namespace Controllers;

[ApiController]
[Route("api/StudentGrades")]
public class StudentGradesController : ControllerBase
{
    private readonly StudentGradesService _service;

    public StudentGradesController(StudentGradesService service)
    {
        _service = service;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subjects = await _service.GetAllAsync();
        return Ok(subjects);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var subject = await _service.GetByIdAsync(id);
        if (subject == null) return NotFound();
        return Ok(subject);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentGradeDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok();
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentGradeDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok();
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}

