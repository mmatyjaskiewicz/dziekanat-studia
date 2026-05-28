using Core.Entities;
using Core.Repositories;
using Infrastructre.Memory;
namespace UnitTest;
public class MemoryGenericRepositoryTest
{
    private readonly IGenericRepositoryAsync<Student> _repo = new MemoryGenericRepository<Student>();
    [Fact]
    public async Task AddStudentToRepositoryTestAsync()
    {
        var expected = new Student
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Email = "adam@wsei.edu.pl",
            NationalId = "00210111111",
            StudentNumber = "S0001",
            YearOfStudy = 1,
            Status = StudentStatus.Active
        };
        var added = await _repo.AddAsync(expected);
        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
        Assert.Equal("Adam", actual?.FirstName);
    }
    [Fact]
    public async Task FindByIdReturnsNullForMissingEntityTest()
    {
        var result = await _repo.FindByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }
    [Fact]
    public async Task FindAllReturnsAllAddedEntitiesTest()
    {
        await _repo.AddAsync(new Student { FirstName = "A", LastName = "B", Email = "a@b.pl" });
        await _repo.AddAsync(new Student { FirstName = "C", LastName = "D", Email = "c@d.pl" });
        var all = (await _repo.FindAllAsync()).ToList();
        Assert.Equal(2, all.Count);
    }
    [Fact]
    public async Task FindPagedReturnsRequestedSliceTest()
    {
        for (var i = 0; i < 5; i++)
        {
            await _repo.AddAsync(new Student
            {
                FirstName = $"F{i}",
                LastName = "Test",
                Email = $"t{i}@t.pl"
            });
        }
        var page1 = await _repo.FindPagedAsync(1, 2);
        var page2 = await _repo.FindPagedAsync(2, 2);
        Assert.Equal(2, page1.Items.Count);
        Assert.Equal(2, page2.Items.Count);
        Assert.Equal(5, page1.TotalCount);
        Assert.Equal(3, page1.TotalPages);
    }
    [Fact]
    public async Task UpdateModifiesEntityInRepositoryTest()
    {
        var student = new Student { FirstName = "Jan", LastName = "Kowalski", Email = "j@k.pl" };
        await _repo.AddAsync(student);
        student.LastName = "Nowak";
        var updated = await _repo.UpdateAsync(student);
        var actual = await _repo.FindByIdAsync(student.Id);
        Assert.Equal("Nowak", actual?.LastName);
        Assert.Equal(updated.Id, actual?.Id);
    }
    [Fact]
    public async Task UpdateThrowsWhenEntityMissingTest()
    {
        var student = new Student { FirstName = "Missing" };
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.UpdateAsync(student));
    }
    [Fact]
    public async Task RemoveByIdDeletesEntityTest()
    {
        var student = new Student { FirstName = "Do", LastName = "Usuniecia", Email = "x@x.pl" };
        await _repo.AddAsync(student);
        await _repo.RemoveByIdAsync(student.Id);
        var actual = await _repo.FindByIdAsync(student.Id);
        Assert.Null(actual);
    }
}

