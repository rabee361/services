using Microsoft.AspNetCore.Mvc;
using Ecommerce.Admin.Models;

namespace Ecommerce.Admin.Controllers;

public class UsersController : BaseAdminController
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public UsersController(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiUrl = (configuration["Services:UsersApi"] ?? "http://localhost:5031");
    }

    public async Task<IActionResult> Index()
    {
        if (!IsAdmin) return Forbidden();
        ViewData["ActivePage"] = "Users";
        
        var response = await _httpClient.GetAsync(_apiUrl);
        var users = await response.Content.ReadFromJsonAsync<List<UserViewModel>>() ?? new List<UserViewModel>();
        
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRole(int id, UserRole role)
    {
        if (!IsAdmin) return Forbidden();
        
        var userResponse = await _httpClient.GetAsync($"{_apiUrl}/{id}");
        if (userResponse.IsSuccessStatusCode)
        {
            var user = await userResponse.Content.ReadFromJsonAsync<UserViewModel>();
            if (user != null)
            {
                user.Role = role;
                await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", user);
            }
        }
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdmin) return Forbidden();
        await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
        return RedirectToAction(nameof(Index));
    }

    private IActionResult Forbidden() => Content("Access Denied");
}
