namespace Core.Exceptions;

// Wyjątek zgłaszany, gdy student o podanym identyfikatorze nie zostanie znaleziony.
public class StudentNotFoundException : Exception
{
    public StudentNotFoundException(string message) : base(message) { }
}
