using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Attributes;
using MyAPI.DTOs;
using MyAPI.DTOs.User;
using MyAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("verify")]
        [Protect]
        public IActionResult Verify()
        {
            var user = HttpContext.Items["User"] as UserModel;
            var dto = _mapper.Map<UserDTO>(user);
            return Ok(new ApiResponse<UserDTO>(dto, "Verify successfully"));
        }
    }
}

