using System.Threading.Tasks;
using MyAPI.DTOs;

namespace MyAPI.Services.Mail
{
    public interface IMailService
    {
        public Task<string> SendRegisterMail(string email);
        public Task<string> SendForgotPasswordEmail(string email);
    }
}
