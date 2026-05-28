using Core.Security;
using Microsoft.AspNetCore.Identity;
namespace Infrastructre.EntityFramework.Entities;
public class AppRole : IdentityRole
{
    public string? Description { get; set; }
    public AppRole() { }
    public AppRole(string roleName, string? description = null) : base(roleName)
    {
        Description = description;
    }
}

