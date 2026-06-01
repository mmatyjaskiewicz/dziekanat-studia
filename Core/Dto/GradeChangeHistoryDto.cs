using Core.Entities;

namespace Core.Dto;

public class GradeChangeHistoryDto
{
    public Guid Id { get; init; }
    public Guid GradeId { get; init; }
    public string ChangedBy { get; init; } = string.Empty;
    public DateTime ChangedAt { get; init; }
    public string Action { get; init; } = string.Empty;
    public GradeValue? OldValue { get; init; }
    public GradeValue? NewValue { get; init; }
    public DateTime? OldIssueDate { get; init; }
    public DateTime? NewIssueDate { get; init; }
}
