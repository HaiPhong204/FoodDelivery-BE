using MyAPI.Services.Mail;
using Microsoft.Extensions.Options;
using MimeKit;
using MyAPI.Models.Setting;

namespace MyAPI.Services
{
    public partial class MailService : IMailService
    {
        private readonly MailSetting _mailSetting;
        public MailService(IOptions<MailSetting> mailSettings)
        {
            _mailSetting = mailSettings.Value;
        }
        public async Task<string> SendForgotPasswordEmail(string email)
        {
            string code = new Random().Next(1000, 9999).ToString();
            try
            {
                MimeMessage mail = new MimeMessage();
                mail.To.Add(MailboxAddress.Parse(email));
                mail.Subject = "T";
                var builder = new BodyBuilder();

                builder.HtmlBody = $"Re-Password Code <h1>{code}</h1>";
                mail.Body = builder.ToMessageBody();
                await this.SendMail(mail);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            return code;
        }
        public async Task<string> SendRegisterMail(string email)
        {
            string code = new Random().Next(1000, 9999).ToString();
            try
            {
                MimeMessage mail = new MimeMessage();
                mail.To.Add(MailboxAddress.Parse(email));
                mail.Subject = "[Food Delivery]Verification Email";
                var builder = new BodyBuilder();

                builder.HtmlBody = $"Verification Code <h1>{code}</h1>";
                mail.Body = builder.ToMessageBody();
                await this.SendMail(mail);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            return code;
        }
    }
}
