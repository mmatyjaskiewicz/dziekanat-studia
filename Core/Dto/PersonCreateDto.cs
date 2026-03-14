namespace Core.Dto;

// DTO przekazywane przy tworzeniu nowej osoby (studenta lub wykładowcy).
public abstract record PersonCreateDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string NationalId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
