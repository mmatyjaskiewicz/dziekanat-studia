namespace Core.Dto;

// DTO skróconego opisu wykładowcy.
public sealed record LecturerSummaryDto : PersonDto
{
    public string Title { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
}

// DTO pełnych danych wykładowcy.
public sealed record LecturerDetailDto : PersonDto
{
    public string Title { get; init; } = string.Empty;
    public string Faculty { get; init; } = string.Empty;
}

// DTO tworzenia nowego wykładowcy.
public sealed record LecturerCreateDto : PersonCreateDto
{
    public string Title { get; init; } = string.Empty;
    public string Faculty { get; init; } = string.Empty;
}

// DTO aktualizacji danych wykładowcy.
public sealed record LecturerUpdateDto : PersonDto
{
    public string Title { get; init; } = string.Empty;
    public string Faculty { get; init; } = string.Empty;
}
