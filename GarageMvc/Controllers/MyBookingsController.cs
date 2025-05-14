using GarageDomain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GarageMvc.Controllers
{
    [Authorize]
    [Route("Booking")]
    public class MyBookingsController : Controller
    {
        private readonly HttpClient _httpClient;
        public MyBookingsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }



        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("My-Bookings")]
        public async Task<IActionResult> MyBookings()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier).ToUpper();

            var response = await _httpClient.GetAsync($"MyBookings/GetAllBookings/{userID}");

            if (!response.IsSuccessStatusCode)
            {

                return View(new List<MyBooking>());
            }
                
            var bookings = await response.Content.ReadFromJsonAsync<List<MyBooking>>();

            return View(bookings);
        }



        [HttpDelete("CancelBooking")]
        public async Task<IActionResult> CancelBooking([FromBody] CancelBooking cancelBooking)
        {
            cancelBooking.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier).ToUpper();
            

            var request = new HttpRequestMessage(HttpMethod.Delete, "MyBookings/CancelBooking")
            {
                Content = JsonContent.Create(cancelBooking)
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new { message = "Failed to cancel booking." });
            }

            bool isCancelled = await response.Content.ReadFromJsonAsync<bool>();
            return Ok(isCancelled);
        }



    }
}
