using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;
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
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, StudentUpdateDto dto)
    {
        var updated = await service.UpdateStudent(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }
    [HttpPost("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(GradeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddGrade(
        [FromRoute] Guid studentId,
        [FromBody] GradeCreateDto dto)
    {
        var note = await service.AddGrade(studentId, dto);
        return CreatedAtAction(nameof(GetGrades), new { studentId }, note);
    }
    [HttpGet("{studentId:guid}/grades")]
    [ProducesResponseType(typeof(IEnumerable<GradeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrades([FromRoute] Guid studentId)
    {
        var grades = await service.GetGrades(studentId);
        return Ok(grades);
    }
    [HttpPut("{studentId:guid}/grades/{gradeId:guid}")]
    public async Task<IActionResult> UpdateGrade(
        [FromRoute] Guid studentId,
        [FromRoute] Guid gradeId,
        [FromBody] GradeUpdateDto dto)
    {
        var updated = await service.UpdateGrade(studentId, gradeId, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }
}

