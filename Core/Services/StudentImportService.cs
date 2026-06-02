using System.Text;
using System.Text.Json;
using Core.Dto;
using Core.Entities;
using Core.Repositories;
using Core.UnitOfWork;
using Core.ValueObjects;
using FluentValidation;
namespace Core.Services;
public class StudentImportService(
    IUniversityUnitOfWork unitOfWork,
    IValidator<StudentCreateDto> validator) : IStudentImportService
{
    public char DetectDelimiter(string sample)
    {
        var candidates = new[] { ';', ',', '\t', '|', '#', ':' };
        char best = ',';
        int bestCount = 0;
        var firstLine = sample.Split('\n', 2)[0];
        foreach (var c in candidates)
        {
            int n = firstLine.Count(ch => ch == c);
            if (n > bestCount)
            {
                bestCount = n;
                best = c;
            }
        }
        return best;
    }
    public async Task<StudentImportReport> ImportAsync(Stream file, string fileName, string importedBy, CancellationToken ct = default)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var rows = ext switch
        {
            ".json" => await ReadJsonAsync(file, ct),
            ".csv" or ".txt" or "" => ReadDelimited(file),
            _ => throw new InvalidOperationException($"Unsupported file extension: {ext}")
        };
        return await ProcessRowsAsync(rows, importedBy, ct);
    }
    private async Task<List<Dictionary<string, string>>> ReadJsonAsync(Stream file, CancellationToken ct)
    {
        var list = new List<Dictionary<string, string>>();
        using var doc = await JsonDocument.ParseAsync(file, cancellationToken: ct);
        if (doc.RootElement.ValueKind != JsonValueKind.Array)
            throw new InvalidOperationException("JSON root must be an array of student objects.");
        foreach (var element in doc.RootElement.EnumerateArray())
        {
            if (element.ValueKind != JsonValueKind.Object)
                continue;
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var prop in element.EnumerateObject())
            {
                dict[prop.Name] = prop.Value.ValueKind switch
                {
                    JsonValueKind.String => prop.Value.GetString() ?? string.Empty,
                    JsonValueKind.Number => prop.Value.GetRawText(),
                    JsonValueKind.True or JsonValueKind.False => prop.Value.GetRawText(),
                    JsonValueKind.Null => string.Empty,
                    _ => prop.Value.GetRawText()
                };
            }
            list.Add(dict);
        }
        return list;
    }
    private List<Dictionary<string, string>> ReadDelimited(Stream file)
    {
        using var reader = new StreamReader(file, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        var content = reader.ReadToEnd();
        if (string.IsNullOrWhiteSpace(content))
            return new List<Dictionary<string, string>>();
        var delimiter = DetectDelimiter(content);
        var lines = content.Replace("\r\n", "\n").Split('\n');
        var nonEmpty = lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        if (nonEmpty.Count == 0) return new List<Dictionary<string, string>>();
        var headers = SplitLine(nonEmpty[0], delimiter);
        var result = new List<Dictionary<string, string>>();
        for (int i = 1; i < nonEmpty.Count; i++)
        {
            var parts = SplitLine(nonEmpty[i], delimiter);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int j = 0; j < headers.Count; j++)
            {
                dict[headers[j].Trim()] = j < parts.Count ? parts[j] : string.Empty;
            }
            result.Add(dict);
        }
        return result;
    }
    private static List<string> SplitLine(string line, char delimiter)
    {
        var result = new List<string>();
        var sb = new StringBuilder();
        bool inQuotes = false;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    sb.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == delimiter && !inQuotes)
            {
                result.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }
        result.Add(sb.ToString());
        return result.Select(v => v.Trim()).ToList();
    }
    private async Task<StudentImportReport> ProcessRowsAsync(List<Dictionary<string, string>> rows, string importedBy, CancellationToken ct)
    {
        var existing = await unitOfWork.Students.FindAllAsync();
        var existingPesels = new HashSet<string>(existing.Select(s => s.NationalId ?? string.Empty), StringComparer.OrdinalIgnoreCase);
        var existingEmails = new HashSet<string>(existing.Select(s => s.Email ?? string.Empty), StringComparer.OrdinalIgnoreCase);
        var report = new StudentImportReport { ImportedBy = importedBy };
        var seenPesels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var seenEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            try
            {
                var dto = MapToDto(row);
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new InvalidOperationException("Email is required.");
                if (string.IsNullOrWhiteSpace(dto.NationalId))
                    throw new InvalidOperationException("PESEL/NationalId is required.");
                if (!Pesel.IsValid(dto.NationalId))
                    throw new InvalidOperationException($"Invalid PESEL checksum: {dto.NationalId}");
                if (existingPesels.Contains(dto.NationalId) || seenPesels.Contains(dto.NationalId))
                    throw new InvalidOperationException($"Student with PESEL {dto.NationalId} already exists.");
                if (existingEmails.Contains(dto.Email) || seenEmails.Contains(dto.Email))
                    throw new InvalidOperationException($"Student with email {dto.Email} already exists.");
                var validation = await validator.ValidateAsync(dto, ct);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
                    report.Failed.Add(new StudentImportRowError(i + 1, row, errors));
                    continue;
                }
                var entity = dto.ToEntity();
                entity = await unitOfWork.Students.AddAsync(entity);
                await unitOfWork.SaveChangesAsync();
                report.Imported.Add(entity.ToSummary());
                seenPesels.Add(dto.NationalId);
                seenEmails.Add(dto.Email);
            }
            catch (Exception ex)
            {
                report.Failed.Add(new StudentImportRowError(i + 1, row, new[] { ex.Message }));
            }
        }
        return report;
    }
    private static StudentCreateDto MapToDto(Dictionary<string, string> row)
    {
        string Get(string key) => row.TryGetValue(key, out var v) ? v.Trim() : string.Empty;
        return new StudentCreateDto
        {
            FirstName = Get("FirstName"),
            LastName = Get("LastName"),
            Email = Get("Email"),
            NationalId = Get("PESEL").Length > 0 ? Get("PESEL") : Get("NationalId"),
            StudentId = Get("StudentId").Length > 0 ? Get("StudentId") : Get("StudentNumber"),
            YearOfStudy = int.TryParse(Get("YearOfStudy"), out var y) ? y : 1,
            ProgramCode = Get("ProgramCode"),
            EnrollmentYearFrom = int.TryParse(Get("EnrollmentYearFrom"), out var ey) ? ey : DateTime.UtcNow.Year
        };
    }
}
