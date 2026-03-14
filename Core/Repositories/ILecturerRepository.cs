using Core.Entities;

namespace Core.Repositories;

// Interfejs repozytorium dla encji Lecturer.
public interface ILecturerRepository : IGenericRepositoryAsync<Lecturer>
{
    Lecturer? FindByCourse(Guid courseId);
    IEnumerable<Lecturer> GetByTitle(string title);
    IEnumerable<Lecturer> GetByFaculty(string faculty);
}
