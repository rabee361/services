using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ecommerce.Admin.Models;
using System.Text.Json;

namespace Ecommerce.Admin.Controllers;

public abstract class BaseAdminController : Controller
{
    protected UserViewModel? CurrentUser { get; private set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userJson = HttpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(userJson))
        {
            context.Result = RedirectToAction("Login", "Account");
            return;
        }

        CurrentUser = JsonSerializer.Deserialize<UserViewModel>(userJson);
        ViewData["CurrentUser"] = CurrentUser;
        base.OnActionExecuting(context);
    }

    protected bool IsAdmin => CurrentUser?.Role == UserRole.Admin;
    protected bool IsManager => CurrentUser?.Role == UserRole.Manager;
}
