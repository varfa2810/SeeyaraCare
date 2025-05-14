using GarageApplication.Interfaces;
using GarageDomain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GarageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyBookingsController : ControllerBase
    {
        private readonly IMyBookings _myBookings;

        public MyBookingsController( IMyBookings myBookings)
        {
            _myBookings = myBookings;   

        }


        [HttpGet("GetAllBookings/{userID}")]
        public async Task<ActionResult<List<MyBooking>>> GetMyBookings(Guid userID)
        {
            if(userID == Guid.Empty)
            {
                return BadRequest("Invalid user ID.");
            }   
            var bookings = await _myBookings.GetMyBookings(userID);
            if (bookings == null || bookings.Count == 0)
            {
                return NotFound("No bookings found for this user.");
            }
            return Ok(bookings);
        }



        [HttpDelete("CancelBooking")]
        public async Task<ActionResult<bool>> CancelBooking(CancelBooking cancelBooking)
        {
            if (cancelBooking.UserID.Equals(cancelBooking.BookingID))
                return BadRequest("Invalid IDs.");

            var isCancelled = await _myBookings.CancelBooking(cancelBooking);
            if (isCancelled)
                return Ok(isCancelled);
            return BadRequest("Error in cancelling booking.");
        }

    }
}
