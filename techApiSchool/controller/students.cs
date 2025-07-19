using domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> Create([FromBody] student student)
    {
        await _service.CreateAsync(student);
        return Ok();
    }

    [Authorize][HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] student student)
    {
        if (id != student.Id) return BadRequest();
        await _service.UpdateAsync(student);
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
