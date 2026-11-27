using Microsoft.AspNetCore.Identity;

namespace 周末作业1.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}
