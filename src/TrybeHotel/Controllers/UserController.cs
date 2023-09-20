using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_repository.GetUsers());
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            var newUser = _repository.Add(user);
            if (newUser == null) return Conflict(new { message = "User email already exists" });
            return Created("", newUser);
        }
    }
}