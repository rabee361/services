using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Ecommerce.Web.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public AccountController(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiUrl = (configuration["Services:UsersApi"] ?? "http://localhost:5031");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string usernameOrEmail, string password)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/login", new { usernameOrEmail, password });
        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("User", user);
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Error = "Invalid credentials";
        return View();
    }

    [HttpGet]
    public IActionResult Signup() => View();

    [HttpPost]
    public async Task<IActionResult> Signup(string username, string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/register", new { username, email, password });
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login");
        }
        ViewBag.Error = "Registration failed. User might already exist.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
