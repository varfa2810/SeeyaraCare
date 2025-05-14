using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using GarageDomain.Entities.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GarageDomain.Entities;
namespace GarageMvc.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult VerifyOtp()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginUser)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid login request.";
                return View();
            }

            try
            {
                var existingOtp = HttpContext.Session.GetString("OTP");
                var expiry = HttpContext.Session.GetString("OtpExpiryTime");


                var response = await _httpClient.PostAsJsonAsync("Authentication/getOtp", new
                {
                    Email = loginUser.Email,
                    Password = loginUser.Password
                });

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Invalid Email or Password.";
                    return View(loginUser);
                }

                string otp = await response.Content.ReadAsStringAsync();
           

                HttpContext.Session.SetString("OTP", otp);
                HttpContext.Session.SetString("Email", loginUser.Email);
                HttpContext.Session.SetString("OtpExpiryTime", DateTime.UtcNow.AddSeconds(60).ToString("o"));


                return RedirectToAction(nameof(VerifyOtp));
            }
            catch
            {
                TempData["Error"] = "Something went wrong while logging in.";
                return View(loginUser);
                
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp([FromForm] OtpRequest otpRequest)
        {

            try
            {

                var otpFromSession = HttpContext.Session.GetString("OTP");
                var emailFromSession = HttpContext.Session.GetString("Email");
                var otpExpiryStr = HttpContext.Session.GetString("OtpExpiryTime");

                if (string.IsNullOrEmpty(otpFromSession))
                {

                    TempData["Error"] = "Invalid OTP.";
                    return View();

                }



                //if (otpRequest.OTP != otpFromSession)
                //{
                //    TempData["Error"] = "OTP dosen't match.";
                //    return View();
                //}

                //if (!string.IsNullOrEmpty(otpExpiryStr) &&
                //    DateTime.TryParse(otpExpiryStr, out var expiryTime) &&
                //    DateTime.UtcNow < expiryTime)
                //{

                //    TempData["Error"] = "OTP expired.";
                //    return View();
                //}



                var response = await _httpClient.PostAsJsonAsync("Authentication/login", new { OTP = otpRequest.OTP, Email = emailFromSession });
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Something went wrong while verifying OTP.";
                    return View();
                }


                HttpContext.Session.Remove("OTP");
                HttpContext.Session.Remove("Email");
                HttpContext.Session.Remove("OtpExpiryTime");


                var token = await response.Content.ReadAsStringAsync();
                

                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                });

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var firstName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (!string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(firstName))
                {
                    var claims = new List<Claim>
                            {

                                new Claim(ClaimTypes.NameIdentifier, userId.ToString().ToUpper()),
                                new Claim(ClaimTypes.Name, firstName),
                                new Claim(ClaimTypes.Role, role)
                            };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                }
                return RedirectToAction("Index", "Home");
            }

            catch
            {
                TempData["Error"] = "Something went wrong while verifying OTP.";
                return View();
            }



        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session"); 
            Response.Cookies.Delete("AuthToken");           
            Response.Cookies.Delete(".AspNetCore.Antiforgery.GarageMvc"); 

            return RedirectToAction("", "Home");
        }



    }
}
