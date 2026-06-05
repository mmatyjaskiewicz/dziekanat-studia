using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

var probePath = @"H:\Projekty\dziekanat-studia\ToolDeleteDb\Probe.cs";
if (File.Exists(probePath)) File.Delete(probePath);

var c = new HttpClient();
c.BaseAddress = new Uri("http://localhost:5000");
var r = await c.PostAsJsonAsync("/api/auth/login", new { email = "admin@app.pl", password = "Admin@123!" });
var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
var token = doc.RootElement.GetProperty("accessToken").GetString();
Console.WriteLine($"token={token!.Substring(0, 20)}...");
c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

var r2 = await c.PostAsJsonAsync("/api/students/00000000-0000-0000-0000-000000000000/assign-program", new { degreeProgramId = Guid.NewGuid(), academicYearId = (Guid?)Guid.NewGuid() });
var s = await r2.Content.ReadAsStringAsync();
Console.WriteLine($"status={(int)r2.StatusCode} body={s.Substring(0, Math.Min(800, s.Length))}");
