using Fe_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Fe_Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDTO);
            }
            var client = _httpClientFactory.CreateClient("BackendApi");
            var jsonContent = JsonConvert.SerializeObject(loginDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/account/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseData);
                if (loginResponse.data.role.name == "admin")
                {
                    HttpContext.Session.SetString("JWTToken", loginResponse.data.token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bạn không có quyền truy cập!");
                    return View(loginDTO);
                }
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Đăng nhập thất bại: {errorResponse}");
                return View(loginDTO);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            HttpContext.Session.Remove("JWTToken");
            return RedirectToAction("Login", "Account");
        }
    }
}
