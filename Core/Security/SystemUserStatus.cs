namespace Core.Security;

// Typ wyliczeniowy statusu użytkownika systemu.
public enum SystemUserStatus
{
    Active = 0,
    Inactive = 1,
    Locked = 2,
    PendingActivation = 3
}
