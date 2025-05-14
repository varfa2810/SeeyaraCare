using GarageApplication.Interfaces;
using GarageDomain.DTOs;
using GarageDomain.Entities;
using GarageDomain.Entities.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GarageApi.Controllers
{
    /// <summary>
    /// Handles vehicle-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthentication _authentication;
        private readonly IConfiguration _configuration;
        public AuthenticationController(IAuthentication authentication, IConfiguration configuration)
        {
            _authentication = authentication;
            _configuration = configuration;
        }

        [SwaggerOperation(Summary = "Only to get OTP from after validating username & password. For token use login route given below.")]
        [HttpPost("getOtp")]
        public async Task<ActionResult<string>> GetOtp([FromBody] LoginRequest loginUser)
        {
            var otp = await _authentication.GetOtp(loginUser);
            if (string.IsNullOrEmpty(otp))
                return Unauthorized("Invalid email or password");


            return Ok(otp);
        }




        [SwaggerOperation(Summary = "Get the JWT token after succesfully validation on OTP & Email.")]
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginViaOTP([FromBody] OtpRequest otpRequest)
        {
            if (string.IsNullOrEmpty(otpRequest.OTP))
                return BadRequest("Invalid OTP.");

            if (string.IsNullOrEmpty(otpRequest.Email))
                return BadRequest("Email is required.");

            var user = await _authentication.CheckUser(otpRequest.Email);

            if (user == null)
                return Unauthorized("User not found.");

            var token = await GenerateJwtToken(user);

            return Ok(token);
        }






        private async Task<string> GenerateJwtToken(UserDTO user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FirstName)
    };

            var role = await _authentication.GetUserRole(user.Id);
            claims.AddRange(role.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        [SwaggerOperation(Summary = "Registers the User for any role.")]

        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] RegisterUser registerUser)
        {
            if (
    registerUser.FirstName?.Trim().ToLower() == "string" ||
    registerUser.LastName?.Trim().ToLower() == "string" ||
    registerUser.Email?.Trim().ToLower() == "user@example.com" ||
    registerUser.DOB >= DateTime.Today
)
                return BadRequest("Invalid values.");

            var result = await _authentication.Register(registerUser);
            if (!result)
            {
                return BadRequest("Registration failed");
            }
            return Ok("Registration successful");
        }

    }
}
