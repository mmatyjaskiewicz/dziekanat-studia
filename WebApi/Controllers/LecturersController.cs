using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/lecturers")]
[Authorize(Policy = "DeanOffice")]
public class LecturersController(ILecturerService lecturerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllLecturers(int page = 1, int size = 10)
    {
        var result = await lecturerService.GetAllLecturersPaged(page, size);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetLecturer(Guid id)
    {
        var dto = await lecturerService.GetLecturerById(id);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LecturerCreateDto dto)
    {
        var result = await lecturerService.AddLecturer(dto);
        return CreatedAtAction(nameof(GetLecturer), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, LecturerUpdateDto dto)
    {
        var updated = await lecturerService.UpdateLecturer(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpGet("{id:guid}/courses")]
    public async Task<IActionResult> GetCourses(Guid id)
    {
        var courses = await lecturerService.GetLecturerCourses(id);
        return Ok(courses);
    }
}
