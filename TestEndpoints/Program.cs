using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

var baseUrl = "http://localhost:5000";
var client = new HttpClient { BaseAddress = new Uri(baseUrl) };
client.Timeout = TimeSpan.FromSeconds(15);

var results = new List<(string name, bool ok, string detail)>();

async Task Test(string name, Func<Task<(bool ok, string detail)>> action)
{
    try
    {
        var (ok, detail) = await action();
        results.Add((name, ok, detail));
        Console.WriteLine($"{(ok ? "OK  " : "FAIL")} | {name} | {detail}");
    }
    catch (Exception ex)
    {
        results.Add((name, false, ex.GetType().Name + ": " + ex.Message));
        Console.WriteLine($"FAIL | {name} | {ex.GetType().Name}: {ex.Message}");
    }
}

await Test("OpenAPI schema (unauthenticated)", async () =>
{
    var r = await client.GetAsync("/openapi/v1.json");
    return (r.StatusCode == HttpStatusCode.OK, $"status={(int)r.StatusCode}");
});

string? adminToken = null;
string? adminRefresh = null;
string? deanToken = null;
string? lectToken = null;
await Test("Login admin", async () =>
{
    var r = await client.PostAsJsonAsync("/api/auth/login", new { email = "admin@app.pl", password = "Admin@123!" });
    if (!r.IsSuccessStatusCode) return (false, $"status={(int)r.StatusCode}, body={await r.Content.ReadAsStringAsync()}");
    var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
    adminToken = doc.RootElement.GetProperty("accessToken").GetString();
    adminRefresh = doc.RootElement.GetProperty("refreshToken").GetString();
    return (adminToken != null, "got admin token");
});
await Test("Login dean", async () =>
{
    var r = await client.PostAsJsonAsync("/api/auth/login", new { email = "jan.kowalski@app.pl", password = "Dean@123!" });
    if (!r.IsSuccessStatusCode) return (false, $"status={(int)r.StatusCode}, body={await r.Content.ReadAsStringAsync()}");
    var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
    deanToken = doc.RootElement.GetProperty("accessToken").GetString();
    return (deanToken != null, "got dean token");
});
await Test("Login lecturer", async () =>
{
    var r = await client.PostAsJsonAsync("/api/auth/login", new { email = "anna.nowak@app.pl", password = "Lect@123!" });
    if (!r.IsSuccessStatusCode) return (false, $"status={(int)r.StatusCode}, body={await r.Content.ReadAsStringAsync()}");
    var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
    lectToken = doc.RootElement.GetProperty("accessToken").GetString();
    return (lectToken != null, "got lecturer token");
});

if (string.IsNullOrEmpty(adminToken)) { Console.WriteLine("no token, abort"); return 1; }
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

await Test("GET /api/auth/me (admin)", async () =>
{
    var r = await client.GetAsync("/api/auth/me");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

Guid studentId = Guid.Empty;
string? studentEmail = $"student{Guid.NewGuid():N}@wsei.edu.pl";
string? studentNumber = $"S{Guid.NewGuid():N}".Substring(0, 12);

await Test("POST /api/students (create)", async () =>
{
    var r = await client.PostAsJsonAsync("/api/students", new
    {
        firstName = "Adam",
        lastName = "Kowalski",
        nationalId = "00210111111",
        email = studentEmail,
        studentId = studentNumber,
        yearOfStudy = 1,
        programCode = "INF",
        enrollmentYearFrom = 2024
    });
    var s = await r.Content.ReadAsStringAsync();
    if (!r.IsSuccessStatusCode) return (false, $"status={(int)r.StatusCode}, body={s}");
    studentId = JsonDocument.Parse(s).RootElement.GetProperty("id").GetGuid();
    return (true, $"created student id={studentId}");
});

await Test("GET /api/students", async () =>
{
    var r = await client.GetAsync("/api/students?page=1&size=5");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

await Test("GET /api/students/{id}", async () =>
{
    if (studentId == Guid.Empty) return (false, "no student");
    var r = await client.GetAsync($"/api/students/{studentId}");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

await Test("PUT /api/students/{id}", async () =>
{
    if (studentId == Guid.Empty) return (false, "no student");
    var r = await client.PutAsJsonAsync($"/api/students/{studentId}", new
    {
        firstName = "Adam",
        lastName = "Kowalski",
        email = studentEmail,
        yearOfStudy = 2,
        status = 1,
        programCode = "INF"
    });
    var s = await r.Content.ReadAsStringAsync();
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}, body={s.Substring(0, Math.Min(120, s.Length))}");
});

Guid lecturerId = Guid.Empty;
string? lecturerEmail = $"jan{Guid.NewGuid():N}@wsei.edu.pl";
await Test("POST /api/lecturers (create)", async () =>
{
    var r = await client.PostAsJsonAsync("/api/lecturers", new
    {
        firstName = "Jan",
        lastName = "Kowalski",
        nationalId = "00210111112",
        email = lecturerEmail,
        title = "dr",
        faculty = "WMI"
    });
    var s = await r.Content.ReadAsStringAsync();
    if (!r.IsSuccessStatusCode) return (false, $"status={(int)r.StatusCode}, body={s}");
    lecturerId = JsonDocument.Parse(s).RootElement.GetProperty("id").GetGuid();
    return (true, $"created lecturer id={lecturerId}");
});

await Test("GET /api/lecturers", async () =>
{
    var r = await client.GetAsync("/api/lecturers?page=1&size=5");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

await Test("GET /api/lecturers/{id}", async () =>
{
    if (lecturerId == Guid.Empty) return (false, "no lecturer");
    var r = await client.GetAsync($"/api/lecturers/{lecturerId}");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

await Test("PUT /api/lecturers/{id}", async () =>
{
    if (lecturerId == Guid.Empty) return (false, "no lecturer");
    var r = await client.PutAsJsonAsync($"/api/lecturers/{lecturerId}", new
    {
        firstName = "Jan",
        lastName = "Kowalski",
        email = lecturerEmail,
        title = "prof.",
        faculty = "WMI"
    });
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

await Test("GET /api/lecturers/{id}/courses", async () =>
{
    if (lecturerId == Guid.Empty) return (false, "no lecturer");
    var r = await client.GetAsync($"/api/lecturers/{lecturerId}/courses");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

Guid courseId = Guid.Empty;
await Test("POST /api/courses (no controller, expect 404/405)", async () =>
{
    var r = await client.PostAsJsonAsync("/api/courses", new
    {
        code = "MAT",
        name = "Matematyka",
        ects = 5
    });
    return (true, $"status={(int)r.StatusCode} (no courses controller exists)");
});

await Test("GET /api/lecturers/{id}/students", async () =>
{
    if (lecturerId == Guid.Empty) return (false, "no lecturer");
    var r = await client.GetAsync($"/api/lecturers/{lecturerId}/students");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

Guid gradeId = Guid.Empty;
Guid courseGradeId = Guid.NewGuid();
Guid lecturerGradeId = Guid.NewGuid();
Guid yearGradeId = Guid.NewGuid();
await Test("POST /api/students/{id}/grades (expect 400/404 - missing course/lecturer/year)", async () =>
{
    if (studentId == Guid.Empty) return (false, "no student");
    var r = await client.PostAsJsonAsync($"/api/students/{studentId}/grades", new
    {
        courseId = courseGradeId,
        lecturerId = lecturerGradeId,
        academicYearId = yearGradeId,
        issueDate = DateTime.UtcNow.AddDays(-1),
        gradeValue = 4
    });
    var s = await r.Content.ReadAsStringAsync();
    return (r.StatusCode == HttpStatusCode.NotFound || r.StatusCode == HttpStatusCode.BadRequest,
        $"status={(int)r.StatusCode}, body={s.Substring(0, Math.Min(150, s.Length))}");
});

await Test("GET /api/students/{id}/grades (empty list)", async () =>
{
    if (studentId == Guid.Empty) return (false, "no student");
    var r = await client.GetAsync($"/api/students/{studentId}/grades");
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

await Test("POST /api/students/{id}/assign-program (expect 404 - no program)", async () =>
{
    if (studentId == Guid.Empty) return (false, "no student");
    var r = await client.PostAsJsonAsync($"/api/students/{studentId}/assign-program", new
    {
        degreeProgramId = Guid.NewGuid(),
        academicYearId = Guid.NewGuid()
    });
    var s = await r.Content.ReadAsStringAsync();
    return (r.StatusCode == HttpStatusCode.NotFound || r.StatusCode == HttpStatusCode.BadRequest,
        $"status={(int)r.StatusCode}, body={s.Substring(0, Math.Min(150, s.Length))}");
});

await Test("POST /api/students/{id}/change-status (success)", async () =>
{
    if (studentId == Guid.Empty) return (false, "no student");
    var r = await client.PostAsJsonAsync($"/api/students/{studentId}/change-status", new
    {
        status = 2,
        reason = "test"
    });
    var s = await r.Content.ReadAsStringAsync();
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}, body={s.Substring(0, Math.Min(150, s.Length))}");
});

await Test("POST /api/students/import (CSV)", async () =>
{
    var csv = "FirstName,LastName,Email,PESEL,StudentId,YearOfStudy,ProgramCode,EnrollmentYearFrom\n" +
              $"Import,Test,import{Guid.NewGuid():N}@wsei.edu.pl,00210111113,S{Guid.NewGuid():N}".Substring(0, 12) + ",1,INF,2024\n";
    using var form = new MultipartFormDataContent();
    var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csv));
    fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
    form.Add(fileContent, "file", "students.csv");
    var r = await client.PostAsync("/api/students/import", form);
    var s = await r.Content.ReadAsStringAsync();
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}, body={s.Substring(0, Math.Min(200, s.Length))}");
});

await Test("POST /api/auth/refresh", async () =>
{
    if (string.IsNullOrEmpty(adminRefresh) || string.IsNullOrEmpty(adminToken)) return (false, "no tokens");
    var r = await client.PostAsJsonAsync("/api/auth/refresh", new { accessToken = adminToken, refreshToken = adminRefresh });
    return (r.IsSuccessStatusCode, $"status={(int)r.StatusCode}");
});

await Test("GET /api/students (no token -> 401)", async () =>
{
    var c2 = new HttpClient { BaseAddress = new Uri(baseUrl) };
    var r = await c2.GetAsync("/api/students");
    return (r.StatusCode == HttpStatusCode.Unauthorized, $"status={(int)r.StatusCode}");
});

await Test("GET /api/students (lecturer -> 403, since not DeanOffice)", async () =>
{
    if (string.IsNullOrEmpty(lectToken)) return (false, "no lecturer token");
    var c2 = new HttpClient { BaseAddress = new Uri(baseUrl) };
    c2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lectToken);
    var r = await c2.GetAsync("/api/students");
    return (r.StatusCode == HttpStatusCode.Forbidden, $"status={(int)r.StatusCode}");
});

int passed = results.Count(r => r.ok);
int total = results.Count;
Console.WriteLine();
Console.WriteLine($"=== {passed}/{total} tests passed ===");
return passed == total ? 0 : 1;
