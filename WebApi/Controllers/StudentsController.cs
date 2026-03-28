using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

// Kontroler REST dla zasobu Student.
[ApiController]
[Route("/api/students")]
public class StudentsController(IStudentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllStudents(int page = 1, int size = 10)
    {
        return Ok(await service.FindAllStudentsPaged(page, size));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudent(Guid id)
    {
        var dto = await service.GetById(id);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        var result = await service.AddStudent(dto);
        return CreatedAtAction(nameof(GetStudent), new { id = result.Id }, result);
    }
}
