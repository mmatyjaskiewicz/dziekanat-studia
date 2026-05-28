using Core.Entities;
using Core.Repositories;
namespace Infrastructre.Memory;
public class InMemoryStudentRepository : MemoryGenericRepository<Student>, IStudentRepository
{
    public InMemoryStudentRepository()
    {
        var adam = new Student
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Email = "adam@wsei.edu.pl",
            NationalId = "00210111111",
            StudentNumber = "S0001",
            YearOfStudy = 1,
            Status = StudentStatus.Active,
            Grades = new List<Grade>()
        };
        var ewa = new Student
        {
            FirstName = "Ewa",
            LastName = "Kowalska",
            Email = "ewa@wsei.edu.pl",
            NationalId = "00310122222",
            StudentNumber = "S0002",
            YearOfStudy = 2,
            Status = StudentStatus.Active,
            Grades = new List<Grade>()
        };
        AddAsync(adam).Wait();
        AddAsync(ewa).Wait();
    }
    public IEnumerable<Student> GetStudentsByStudyYear(int studyYear)
    {
        return _data.Values.Where(s => s.YearOfStudy == studyYear).ToList();
    }
    public IEnumerable<Student> GetStudentsByProgram(Guid programId)
    {
        return _data.Values.Where(s => s.DegreeProgramId == programId).ToList();
    }
    public Task ChangeStatusAsync(Guid studentId, StudentStatus status)
    {
        if (!_data.TryGetValue(studentId, out var student))
            throw new KeyNotFoundException($"Student with id={studentId} not found.");
        student.Status = status;
        return Task.CompletedTask;
    }
}

