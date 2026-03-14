namespace Core.Entities;

// Bazowa klasa encji dostarczająca klucz typu Guid dla wszystkich klas dziedziczących.
public abstract class EntityBase
{
    public Guid Id { get; set; }
}
