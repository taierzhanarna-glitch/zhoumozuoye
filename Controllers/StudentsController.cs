using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using 周末作业1.Models;

namespace 周末作业1.Controllers;

[Authorize]
public class StudentsController(UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var students = await userManager.GetUsersInRoleAsync("Student");
        return View(students.OrderBy(user => user.LastName).ThenBy(user => user.FirstName));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            TempData["Error"] = "删除用户失败，请稍后重试。";
        }

        return RedirectToAction(nameof(Index));
    }
}
