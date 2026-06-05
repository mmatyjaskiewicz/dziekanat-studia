using Core.Entities;
namespace Core.Entities;
public class Lecturer : Person
{
    public string Title { get; set; } = string.Empty;  
    public string Faculty { get; set; } = string.Empty; 
    public List<Course> Courses { get; set; } = new();
}