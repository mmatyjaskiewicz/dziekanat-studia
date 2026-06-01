using Core.Dto;
using Core.Entities;
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
        var changedBy = User.Identity?.Name ?? "system";
        var note = await service.AddGrade(studentId, dto, changedBy);
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

    [HttpGet("{studentId:guid}/grades/{gradeId:guid}/history")]
    public async Task<IActionResult> GetGradeHistory([FromRoute] Guid studentId, [FromRoute] Guid gradeId)
    {
        var history = await service.GetGradeHistory(gradeId);
        return Ok(history);
    }

    public record AssignProgramRequest(Guid DegreeProgramId, Guid? AcademicYearId);
    [HttpPost("{id:guid}/assign-program")]
    public async Task<IActionResult> AssignToProgram(Guid id, [FromBody] AssignProgramRequest request)
    {
        await service.AssignToProgram(id, request.DegreeProgramId, request.AcademicYearId);
        return NoContent();
    }

    public record ChangeStatusRequest(StudentStatus Status, string Reason);
    [HttpPost("{id:guid}/change-status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest request)
    {
        await service.ChangeStatus(id, request.Status, request.Reason);
        return NoContent();
    }
    [HttpPut("{studentId:guid}/grades/{gradeId:guid}")]
    public async Task<IActionResult> UpdateGrade(
        [FromRoute] Guid studentId,
        [FromRoute] Guid gradeId,
        [FromBody] GradeUpdateDto dto)
    {
        var changedBy = User.Identity?.Name ?? "system";
        var updated = await service.UpdateGrade(studentId, gradeId, dto, changedBy);
        if (updated is null) return NotFound();
        return Ok(updated);
    }
}

