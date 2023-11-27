using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyAPI.Core.IRepositories;
using MyAPI.DTOs.User;
using MyAPI.Error;
using MyAPI.Models;
using MyAPI.Services.Mail;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAPI.Core.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        public DataContext _context { get; set; }
        private static IDictionary<string, CodeModel> ListTokenAccount = new Dictionary<string, CodeModel>();
        private static IDictionary<string, CodeModel> ListForgotPasswordAccount = new Dictionary<string, CodeModel>();
        private static IDictionary<string, string> ListResetPasswordAccount = new Dictionary<string, string>();

        public AuthRepository(IConfiguration configuration, DataContext context, IMapper mapper, IMailService mailService)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
        }

        public string GenerateToken(UserModel user)
        {

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim("UserId", user.Id.ToString())
              }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        public string? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public async Task<(UserModel, string)> Login(LoginDTO UserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(UserDto.Email.ToLower()));
            if (user == null)
            {
                throw new ApiException("User not found!", 400);
            }
            else if (user.Password != UserDto.Password)
            {
                throw new ApiException("Wrong password!", 400);
            }
            return (user, GenerateToken(user));
        }

        public async Task Register(RegisterUserDTO userDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(userDTO.Email.ToLower()));
            if (user != null)
            {
                throw new ApiException("Email have already existed!", 400);
            }
            CodeModel codeModel;
            if (ListTokenAccount.TryGetValue(userDTO.Email, out codeModel!))
            {
                if (codeModel.ExpiredAt > DateTime.Now)
                {
                    throw new ApiException("Please try again in 2 minutes", 400);
                }
                ListTokenAccount.Remove(userDTO.Email);
            }
            var tokenCode = await _mailService.SendRegisterMail(userDTO.Email);
            UserModel _user = _mapper.Map<UserModel>(userDTO);
            var code = new CodeModel { Value = tokenCode, ExpiredAt = DateTime.Now.AddMinutes(2), User = _user };
            ListTokenAccount.Add(userDTO.Email, code);
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                throw new ApiException("User not found.", 400);
            }
            CodeModel codeModel;
            if (ListForgotPasswordAccount.TryGetValue(email, out codeModel!))
            {
                if (codeModel.ExpiredAt > DateTime.Now)
                {
                    throw new ApiException("Please try again in 2 minutes", 400);
                }
                ListForgotPasswordAccount.Remove(email);
            }
            var rePasswordCode = await _mailService.SendForgotPasswordEmail(email);
            ListForgotPasswordAccount.Add(email, new CodeModel { Value = rePasswordCode, ExpiredAt = DateTime.Now.AddMinutes(2) });
        }

        public async Task<bool> FindUserByEmai(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower())) != null;
        }

        public async Task<(UserModel, string)> VerifyEmailToken(CodeDTO codeDTO)
        {
            CodeModel codeModel;
            if (!ListTokenAccount.TryGetValue(codeDTO.Email, out codeModel!) || codeDTO.Value != codeModel.Value || codeModel.ExpiredAt < DateTime.Now)
            {
                throw new ApiException("Code is wrong or expired!", 400);
            }
            UserModel user = new UserModel
            {
                Email = codeModel.User.Email,
                Password = codeModel.User.Password,
                IsVerified = true
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            ListTokenAccount.Remove(codeDTO.Email);
            return (user, GenerateToken(user));
        }

        public async Task<(UserModel, string)> ResetPassword(ResetPasswordDTO UserDto)
        {
            string email;
            if (!ListResetPasswordAccount.TryGetValue(UserDto.Value, out email!))
            {
                throw new ApiException("Invalid Token", 400);
            }
            var _user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (_user == null)
            {
                throw new ApiException("User not found.", 400);
            }
            ListResetPasswordAccount.Remove(UserDto.Value);
            _user.Password = UserDto.Password;
            await _context.SaveChangesAsync();
            return (_user, GenerateToken(_user));
        }

        public string VerifyResetPassword(string email, string value)
        {
            CodeModel codeModel;
            if (!ListForgotPasswordAccount.TryGetValue(email, out codeModel!) || value != codeModel.Value)
            {
                throw new ApiException("Code is wrong or expired!", 400);
            }
            ListForgotPasswordAccount.Remove(email);
            string code = CreateRandomCode();
            ListResetPasswordAccount.Add(code, email);
            return code;
        }

        public async Task ChangePassword(ChangePasswordDTO changePassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(changePassword.Email.ToLower()));
            if (user == null)
            {
                throw new ApiException("User not found.", 400);
            }
            else if (user.Password != changePassword.Password)
            {
                throw new ApiException("Wrong current password!", 400);
            }
            user.Password = changePassword.Password;
            await _context.SaveChangesAsync();
        }

        private string CreateRandomCode()
        {
            return new Random().Next(1000, 9999).ToString("D4");
        }
    }
}
