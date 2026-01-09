using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Admin.Controllers;

public class DashboardController : BaseAdminController
{
    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Dashboard";
        return View();
    }
}
