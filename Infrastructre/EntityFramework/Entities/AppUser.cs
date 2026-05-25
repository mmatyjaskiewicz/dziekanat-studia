using Core.Security;
using Microsoft.AspNetCore.Identity;

namespace Infrastructre.EntityFramework.Entities;

// Klasa użytkownika systemu dziekanatu - dziedziczy po IdentityUser i implementuje ISystemUser.
public class AppUser : IdentityUser, ISystemUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }
    public new required string Email { get; set; }
    public required string Department { get; set; }
    public SystemUserStatus Status { get; set; } = SystemUserStatus.PendingActivation;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }

    public void Activate()
    {
        if (Status == SystemUserStatus.Inactive)
        {
            Status = SystemUserStatus.Active;
            DeactivatedAt = null;
        }
    }

    public void Deactivate(DateTime now)
    {
        if (Status == SystemUserStatus.Active)
        {
            Status = SystemUserStatus.Inactive;
            DeactivatedAt = now;
        }
    }
}
