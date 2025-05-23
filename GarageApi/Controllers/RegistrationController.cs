using GarageApplication.Interfaces;
using GarageDomain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistration _registration;

        public RegistrationController(IRegistration registration)
        {
            _registration = registration;
            
        }

        [HttpPost("Register")]
        public async Task<ActionResult<bool>> Register(Register register)
        {
            bool result  = await _registration.Register(register);
            if (result) 
                return Ok("User registered successfuly.");
            else 
                return BadRequest();
                
            
        }
    }
}
