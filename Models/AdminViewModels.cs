using System.ComponentModel.DataAnnotations;

namespace 周末作业1.Models;

public class UserRoleViewModel
{
    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string CurrentRole { get; set; } = "Student";
}

public class UpdateUserRoleViewModel
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "请选择角色")]
    [RegularExpression("^(Admin|Student)$", ErrorMessage = "角色值无效")]
    public string Role { get; set; } = "Student";
}
