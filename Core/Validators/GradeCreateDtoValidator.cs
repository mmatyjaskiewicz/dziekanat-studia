using Core.Dto;
using FluentValidation;
namespace Core.Validators;
public class GradeCreateDtoValidator : AbstractValidator<GradeCreateDto>
{
    public GradeCreateDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Identyfikator kursu jest wymagany.");
        RuleFor(x => x.LecturerId)
            .NotEmpty().WithMessage("Identyfikator prowadzÄ…cego jest wymagany.");
        RuleFor(x => x.AcademicYearId)
            .NotEmpty().WithMessage("Identyfikator roku akademickiego jest wymagany.");
        RuleFor(x => x.IssueDate)
            .NotEmpty().WithMessage("Data wystawienia jest wymagana.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data wystawienia nie moÅ¼e byÄ‡ z przyszłoÅ›ci.");
        RuleFor(x => x.GradeValue)
            .IsInEnum().WithMessage("Niepoprawna wartoÅ›Ä‡ oceny.");
    }
}

