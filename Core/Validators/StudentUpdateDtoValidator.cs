using Core.Dto;
using FluentValidation;
namespace Core.Validators;
public class StudentUpdateDtoValidator : AbstractValidator<StudentUpdateDto>
{
    public StudentUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("ImiĂ„â„˘ jest wymagane.")
            .MaximumLength(100).WithMessage("ImiĂ„â„˘ nie moĂ…ÂĽe przekraczaĂ„â€ˇ 100 znakĂÂłw.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("ImiĂ„â„˘ zawiera niedozwolone znaki.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie moĂ…ÂĽe przekraczaĂ„â€ˇ 200 znakĂÂłw.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("NieprawidĂ…â€šowy format adresu email.");
        RuleFor(x => x.YearOfStudy)
            .Must(year => year >= 1 && year <= 5)
            .WithMessage("Niepoprawny rok studiĂÂłw.");
        RuleFor(x => x.ProgramCode)
            .NotEmpty().WithMessage("Kod kierunku jest wymagany.");
    }
}

