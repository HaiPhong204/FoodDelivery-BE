using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DTOs.User;
using MyAPI.Models;
using MyAPI.Error;
using MyAPI.Core.IRepositories;
using MyAPI.DTOs;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _auth;
        private readonly IMapper _mapper;

        public AuthController(IMapper mapper, IAuthRepository auth)
        {
            _auth = auth;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginUser)
        {
            (UserModel? user, string? token) = await _auth.Login(loginUser);
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Token = token;
            return Ok(new ApiResponse<UserDTO>(userDTO, "Login successfully"));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDTO user)
        {
            await _auth.Register(user);
            return Ok(new ApiResponse<string>(String.Empty, "Send email verification successfully"));
        }

        [HttpPost("verify-account")]
        [Produces(typeof(ApiResponse<UserDTO>))]
        public async Task<IActionResult> VerifyEmailToken([FromBody] CodeDTO codeDTO)
        {
            (UserModel user, string token) = await _auth.VerifyEmailToken(codeDTO);
            var userDTO = _mapper.Map<UserDTO>(user); userDTO.Token = token;
            return Ok(new ApiResponse<UserDTO>(userDTO, "Verify account successfully!"));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailUserDTO user)
        {
            await _auth.ForgotPassword(user.Email);
            return Ok(new ApiResponse<string>(string.Empty, "Please check the code in your email. This code consists of 4 numbers."));
        }

        [HttpPost("verify-code-repassword")]
        public IActionResult VerifyResetPassword([FromBody] TokenVerificationDTO user)
        {
            string token = _auth.VerifyResetPassword(user.Email, user.Code);
            return Ok(new ApiResponse<string>(token, "Verify code successfully!"));
        }

        [HttpPost("reset-password")]
        [Produces(typeof(ApiResponse<UserDTO>))]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO reUser)
        {
            (UserModel user, string token) = await _auth.ResetPassword(reUser);
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Token = token;
            return Ok(new ApiResponse<UserDTO>(userDTO, "Reset Password successfully."));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO chUser)
        {
            await _auth.ChangePassword(chUser);
            return Ok(new ApiResponse<string>(string.Empty, "Change Password successfully."));
        }
    }
}

