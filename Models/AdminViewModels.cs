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

public class CreateUserViewModel
{
    [Required(ErrorMessage = "请输入邮箱")]
    [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
    [Display(Name = "邮箱")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "请输入名字")]
    [StringLength(50, ErrorMessage = "名字不能超过 50 个字符")]
    [Display(Name = "名字")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "请输入姓氏")]
    [StringLength(50, ErrorMessage = "姓氏不能超过 50 个字符")]
    [Display(Name = "姓氏")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "请输入密码")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "密码至少需要 8 个字符")]
    [DataType(DataType.Password)]
    [Display(Name = "密码")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "请确认密码")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
    [Display(Name = "确认密码")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "请选择角色")]
    [RegularExpression("^(Admin|Student)$", ErrorMessage = "角色值无效")]
    [Display(Name = "角色")]
    public string Role { get; set; } = "Student";
}
