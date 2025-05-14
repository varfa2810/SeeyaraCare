using GarageApplication.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHome _home;
        public HomeController(IHome home)
        {
            _home = home;
        }


        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _home.GetAllProducts();
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }
    }
}
