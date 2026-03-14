namespace Core.Dto;

// DTO dla odpowiedzi na temat osoby (wykorzystywane m.in. przez Student i Lecturer).
public abstract record PersonDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
