using Microsoft.AspNetCore.Mvc;
using Ecommerce.Admin.Models;
using System.Text.Json;

namespace Ecommerce.Admin.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var usersApiUrl = _configuration["Services:UsersApi"] ?? "http://localhost:5031";
        var response = await _httpClient.PostAsJsonAsync($"{usersApiUrl}/api/users/login", model);

        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<UserViewModel>();
            if (user != null)
            {
                if (user.Role == UserRole.Admin || user.Role == UserRole.Manager)
                {
                    HttpContext.Session.SetString("User", JsonSerializer.Serialize(user));
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Access denied. Only Admins and Managers can access this portal.");
                }
            }
        }
        else
        {
            ModelState.AddModelError("", "Invalid login attempt.");
        }

        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
