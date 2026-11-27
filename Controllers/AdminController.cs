using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 周末作业1.Models;

namespace 周末作业1.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : Controller
{
    private static readonly string[] ManagedRoles = ["Admin", "Student"];

    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var users = await userManager.Users
            .OrderBy(user => user.Email)
            .ToListAsync();
        var result = new List<UserRoleViewModel>(users.Count);

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            result.Add(new UserRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email ?? user.UserName ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                CurrentRole = roles.FirstOrDefault(role => ManagedRoles.Contains(role)) ?? "Student"
            });
        }

        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRole(UpdateUserRoleViewModel model)
    {
        if (!ModelState.IsValid || !ManagedRoles.Contains(model.Role))
        {
            TempData["RoleError"] = "角色值无效，只能选择 Admin 或 Student。";
            return RedirectToAction(nameof(Users));
        }

        var user = await userManager.FindByIdAsync(model.UserId);
        if (user is null)
        {
            TempData["RoleError"] = "找不到该用户。";
            return RedirectToAction(nameof(Users));
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        var currentManagedRole = currentRoles.FirstOrDefault(role => ManagedRoles.Contains(role));
        if (currentManagedRole == model.Role)
        {
            TempData["RoleMessage"] = "用户角色没有变化。";
            return RedirectToAction(nameof(Users));
        }

        var currentUserId = userManager.GetUserId(User);
        if (model.Role == "Student" && user.Id == currentUserId && currentManagedRole == "Admin")
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            if (admins.Count <= 1)
            {
                TempData["RoleError"] = "不能移除系统中最后一个 Admin。请先设置另一位 Admin。";
                return RedirectToAction(nameof(Users));
            }
        }

        if (currentRoles.Count > 0)
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                AddErrors(removeResult);
                return RedirectToAction(nameof(Users));
            }
        }

        if (!await roleManager.RoleExistsAsync(model.Role))
        {
            TempData["RoleError"] = $"角色 {model.Role} 不存在。";
            return RedirectToAction(nameof(Users));
        }

        var addResult = await userManager.AddToRoleAsync(user, model.Role);
        if (!addResult.Succeeded)
        {
            AddErrors(addResult);
        }
        else
        {
            TempData["RoleMessage"] = $"已将 {user.Email} 的角色更新为 {model.Role}。请重新登录以刷新权限。";
        }

        return RedirectToAction(nameof(Users));
    }

    private void AddErrors(IdentityResult result)
    {
        TempData["RoleError"] = string.Join(" ", result.Errors.Select(error => error.Description));
    }
}
