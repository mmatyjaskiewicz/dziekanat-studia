namespace Core.Entities;

// Abstrakcyjna klasa bazowa dla osób (Student, Lecturer).
// Wspólne właściwości: imię, nazwisko, email, narodowy identyfikator.
public abstract class Person : EntityBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
}
