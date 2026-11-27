# ASP.NET Core Identity 学生管理平台

本项目使用 ASP.NET Core MVC（目标框架 .NET 10）实现用户注册、登录、退出和基于角色的访问控制。

## 功能

- `ApplicationUser` 扩展 `IdentityUser`，保存 FirstName 和 LastName。
- SQLite 数据库保存 Identity 用户、角色和声明数据。
- 新注册用户自动获得 `Student` 角色。
- `Student` 和 `Admin` 都可以查看学生列表。
- 只有 `Admin` 可以通过受 CSRF 保护的 POST 请求删除学生。
- 密码由 ASP.NET Core Identity 哈希保存，不在代码中保存任何密码。

## 运行步骤

1. 安装 .NET 10 SDK。
2. 在项目目录执行 `dotnet restore`。
3. 执行 `dotnet run`。应用启动时会自动执行迁移并创建 `Admin`、`Student` 角色。
4. 访问应用后注册一个账户，注册成功后即可查看学生列表。

数据库连接在 `appsettings.json` 中指向本地 `app.db`。该文件已加入 `.gitignore`，不会提交到仓库。生产环境请通过环境变量或 Secret Manager 配置连接字符串和其他敏感值。

## 管理员测试

项目不会在源代码中硬编码管理员密码。要测试 Admin 权限，可以在开发数据库中使用 EF Core 或受保护的管理工具将用户加入 `Admin` 角色，例如在应用服务范围内调用：

```csharp
await userManager.AddToRoleAsync(user, "Admin");
```

测试结果应包括：未登录用户访问 `/Students` 被重定向到登录页；Student 能查看但不能删除；Admin 能查看并删除。

## 迁移

初始迁移位于 `Data/Migrations`。手动更新数据库可执行：

```bash
dotnet ef database update
```

## 提交材料建议

运行应用后分别截取注册、登录、Student 无权删除和 Admin 允许删除页面，作为作业截图。遇到问题时可查看 ASP.NET Core 日志和页面 `ModelState` 验证错误。
