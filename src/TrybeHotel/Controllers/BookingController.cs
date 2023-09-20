using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            BookingResponse newBooking = _repository.Add(bookingInsert, email!);

            if (newBooking == null)
            {
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }

            return Created("", newBooking);
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid)
        {
            string email = (HttpContext.User.Identity as ClaimsIdentity)!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value!;

            BookingResponse retrievedBooking = _repository.GetBooking(Bookingid, email!);

            if (retrievedBooking == null) return Unauthorized();

            return Ok(retrievedBooking);
        }
    }
}