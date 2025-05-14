using GarageDomain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GarageMvc.Controllers
{

    [Route("Product")]
    public class ProductDetailsController : Controller
    {

        private readonly HttpClient _httpClient;

        public ProductDetailsController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");

        }

        public IActionResult Index()
        {

            return View();
        }



        [AllowAnonymous]
        [HttpGet("Product-Details/{productID}")]
        public async Task<IActionResult> ProductDetails(Guid productID)
        {
            var result = await _httpClient.GetAsync($"ProductDetails/GetProductDetailsById/{productID}");
            if (result.IsSuccessStatusCode)
            {
                var productDetails = await result.Content.ReadFromJsonAsync<Products>();
                return View(productDetails);
            }

            return View();
        }



        [Authorize(Roles = "User")]
        [HttpPost("BookProduct")]
        public async Task<IActionResult> BookProduct([FromBody] BookProduct bookProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please fill in valid details.");

            bookProduct.UserID = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            try
            {

                var response = await _httpClient.PostAsJsonAsync($"ProductDetails/BookProduct", bookProduct);

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return View(bookProduct);
                }
            }
            catch (Exception ex)
            {

                return View(bookProduct);
            }
        }






    }
}
