using Core.Dto;
using FluentValidation;
namespace Core.Validators;
public class PersonCreateDtoValidator : AbstractValidator<PersonCreateDto>
{
    public PersonCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("ImiĂ„â„˘ jest wymagane.")
            .MaximumLength(100).WithMessage("ImiĂ„â„˘ nie moĂ…ÂĽe przekraczaĂ„â€ˇ 100 znakĂÂłw.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("ImiĂ„â„˘ zawiera niedozwolone znaki.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie moĂ…ÂĽe przekraczaĂ„â€ˇ 200 znakĂÂłw.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwisko zawiera niedozwolone znaki.");
        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("Numer PESEL jest wymagany.")
            .Length(11).WithMessage("Numer PESEL musi mieĂ„â€ˇ 11 znakĂÂłw.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("NieprawidĂ…â€šowy format adresu email.")
            .MaximumLength(200);
    }
}

