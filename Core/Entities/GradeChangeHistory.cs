namespace Core.Entities;

public class GradeChangeHistory : EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid GradeId { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string Action { get; set; } = string.Empty;
    public GradeValue? OldValue { get; set; }
    public GradeValue? NewValue { get; set; }
    public DateTime? OldIssueDate { get; set; }
    public DateTime? NewIssueDate { get; set; }
}
