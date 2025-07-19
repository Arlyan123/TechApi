using domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services;

namespace Controllers;

[ApiController]
[Route("api/Students")]
public class StudentsController : ControllerBase
{
    private readonly StudentService _service;
    public StudentsController(StudentService service) => _service = service;

    [Authorize][HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(id));

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

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound();

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
