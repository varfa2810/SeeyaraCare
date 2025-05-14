using GarageApplication.Interfaces;
using GarageDomain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {

        private readonly IProductDetails _productDetails;

        public ProductDetailsController(IProductDetails productDetails)
        {
            _productDetails = productDetails;
        }



        [HttpGet("GetProductDetailsById/{productID}")]
        public async Task<IActionResult> GetProductDetailsById(Guid productID)
        {
            if (productID == Guid.Empty)
            {
                return BadRequest("Invalid product ID.");
            }
            var productDetails = await _productDetails.GetProductDetailsById(productID);
            if (productDetails == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(productDetails);
        }


        [HttpPost("BookProduct")]
        public async Task<IActionResult> BookProduct([FromBody] BookProduct bookProduct)
        {
            if (bookProduct == null || bookProduct.TotalPrice == 0 || bookProduct.Quantity == 0 || bookProduct.ProductID.Equals(bookProduct.UserID))
            {
                return BadRequest("Invalid booking details.");
            }
            var result = await _productDetails.BookProduct(bookProduct);
            if (result)
            {
                return Ok("Booking successful.");
            }
            return BadRequest("Booking failed.");
        }                                           


    }
}
