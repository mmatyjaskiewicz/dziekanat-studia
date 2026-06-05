using Core.Entities;
namespace Core.Repositories;
public interface IStudentRepository : IGenericRepositoryAsync<Student>
{
    IEnumerable<Student> GetStudentsByStudyYear(int studyYear);
    IEnumerable<Student> GetStudentsByProgram(Guid programId);
    IEnumerable<Student> GetStudentsWithGradeByLecturer(Guid lecturerId);
    Task ChangeStatusAsync(Guid studentId, StudentStatus status);
}

