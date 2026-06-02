namespace Core.Dto;
public sealed record StudentImportRowError(int RowIndex, Dictionary<string, string> Data, string[] Errors);
public sealed record StudentImportReport
{
    public List<StudentSummaryDto> Imported { get; init; } = new();
    public List<StudentImportRowError> Failed { get; init; } = new();
    public int TotalRows => Imported.Count + Failed.Count;
    public string ImportedBy { get; init; } = string.Empty;
}
