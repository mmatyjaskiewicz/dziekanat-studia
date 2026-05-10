using Core.Dto;
using FluentValidation;

namespace Core.Validators;

// Walidator dla danych wejściowych oceny.
public class GradeCreateDtoValidator : AbstractValidator<GradeCreateDto>
{
    public GradeCreateDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Identyfikator kursu jest wymagany.");

        RuleFor(x => x.LecturerId)
            .NotEmpty().WithMessage("Identyfikator prowadzącego jest wymagany.");

        RuleFor(x => x.AcademicYearId)
            .NotEmpty().WithMessage("Identyfikator roku akademickiego jest wymagany.");

        RuleFor(x => x.IssueDate)
            .NotEmpty().WithMessage("Data wystawienia jest wymagana.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data wystawienia nie może być z przyszłości.");

        RuleFor(x => x.GradeValue)
            .IsInEnum().WithMessage("Niepoprawna wartość oceny.");
    }
}
