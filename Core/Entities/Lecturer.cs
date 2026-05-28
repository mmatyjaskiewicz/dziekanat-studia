using Core.Entities;
namespace Core.Entities;
public class Lecturer : Person
{
    public string Title { get; set; } = string.Empty;   // np. dr, dr hab., prof.
    public string Faculty { get; set; } = string.Empty; // wydział
    public List<Course> Courses { get; set; } = new();  // kursy prowadzone przez wykładowcÄ™
}

