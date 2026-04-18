using Core.Dto;
using FluentValidation;

namespace Core.Validators;

// Walidator dla StudentUpdateDto.
public class StudentUpdateDtoValidator : AbstractValidator<StudentUpdateDto>
{
    public StudentUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(100).WithMessage("Imię nie może przekraczać 100 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Imię zawiera niedozwolone znaki.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie może przekraczać 200 znaków.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("Nieprawidłowy format adresu email.");

        RuleFor(x => x.YearOfStudy)
            .Must(year => year >= 1 && year <= 5)
            .WithMessage("Niepoprawny rok studiów.");

        RuleFor(x => x.ProgramCode)
            .NotEmpty().WithMessage("Kod kierunku jest wymagany.");
    }
}
