using MyAPI.DTOs.User;
using MyAPI.Models;

namespace MyAPI.Core.IRepositories
{
    public interface IAuthRepository
    {
        string GenerateToken(UserModel user);
        string? ValidateToken(string token);
        public Task<(UserModel, string)> Login(LoginDTO UserDto);
        public Task Register(RegisterUserDTO userDTO);
        public Task ForgotPassword(string emai);
        public Task<bool> FindUserByEmai(string email);
        public Task<(UserModel, string)> VerifyEmailToken(CodeDTO codeDTO);
        public Task<(UserModel, string)> ResetPassword(ResetPasswordDTO UserDto);
        public string VerifyResetPassword(string email, string value);
        public Task ChangePassword(ChangePasswordDTO changePassword);
    }
}
