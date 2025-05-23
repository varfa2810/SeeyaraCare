using GarageDomain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageMvc.Controllers
{
    [AllowAnonymous]
    [Route("RegistratioonUser")]
    public class RegistrationController : Controller
    {
        private readonly HttpClient _httpClient;

        public RegistrationController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] Register register)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", register);
            }

            try
            {
                var result = await _httpClient.PostAsJsonAsync($"Registration/Register", register);
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Authentication");
                }
                else
                {
                  
                    return View("Index", register);
                }
            }
            catch (Exception ex)
            {
                
                return View("Index", register);
            }
        }



    }
}
