using System;

namespace Core.Security;

// Interfejs z właściwościami użytkownika systemu.
// W module Infrastructure klasa encji użytkownika implementuje ten interfejs.
public interface ISystemUser
{
    string Id { get; }
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    string FullName { get; }
    string Department { get; }
    SystemUserStatus Status { get; }
    DateTime CreatedAt { get; }
}
