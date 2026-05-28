namespace Core.Authorization;
public enum AppPolicies
{
    AdminOnly,
    DeanOffice,
    LecturerOnly,
    StudentOnly,
    ActiveUser
}
public static class AppPoliciesExtensions
{
    public static string Name(this AppPolicies policy) => policy.ToString();
}

