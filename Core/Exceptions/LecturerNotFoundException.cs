namespace Core.Exceptions;

// Wyjątek zgłaszany, gdy wykładowca o podanym identyfikatorze nie zostanie znaleziony.
public class LecturerNotFoundException : Exception
{
    public LecturerNotFoundException(string message) : base(message) { }
}
