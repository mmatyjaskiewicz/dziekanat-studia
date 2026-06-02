using Core.Dto;
namespace Core.Services;
public interface IStudentImportService
{
    Task<StudentImportReport> ImportAsync(Stream file, string fileName, string importedBy, CancellationToken ct = default);
    char DetectDelimiter(string sample);
}
