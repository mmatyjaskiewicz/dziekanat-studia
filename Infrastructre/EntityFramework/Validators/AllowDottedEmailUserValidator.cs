using Microsoft.AspNetCore.Identity;
namespace Infrastructre.EntityFramework.Validators;
public class AllowDottedEmailUserValidator<TUser> : IUserValidator<TUser> where TUser : class
{
    private readonly UserManager<TUser> _manager;
    public AllowDottedEmailUserValidator(UserManager<TUser> manager)
    {
        _manager = manager;
    }
    public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
    {
        var errors = new List<IdentityError>();
        var userName = await manager.GetUserNameAsync(user);
        if (string.IsNullOrWhiteSpace(userName))
            errors.Add(new IdentityError { Code = "InvalidUserName", Description = "User name cannot be empty." });
        else if (!IsValidUserName(userName))
            errors.Add(new IdentityError { Code = "InvalidUserName", Description = $"User name '{userName}' is invalid." });
        var email = await manager.GetEmailAsync(user);
        if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
            errors.Add(new IdentityError { Code = "InvalidEmail", Description = $"Email '{email}' is invalid." });
        if (manager.Options.User.RequireUniqueEmail && !string.IsNullOrEmpty(email))
        {
            var owner = await manager.FindByEmailAsync(email);
            if (owner is not null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                errors.Add(new IdentityError { Code = "DuplicateEmail", Description = $"Email '{email}' is already taken." });
        }
        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }
    private static bool IsValidUserName(string name)
    {
        foreach (var c in name)
        {
            if (!(char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '-' || c == '@'))
                return false;
        }
        return name.Length > 0;
    }
    private static bool IsValidEmail(string email)
    {
        var at = email.IndexOf('@');
        if (at <= 0 || at == email.Length - 1) return false;
        if (email.IndexOf('@', at + 1) != -1) return false;
        return email.IndexOf('.', at) != -1;
    }
}
