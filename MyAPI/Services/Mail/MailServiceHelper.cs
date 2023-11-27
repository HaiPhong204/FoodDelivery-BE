using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyAPI.Models.Setting;

namespace MyAPI.Services
{
    public partial class MailService
    {
        public async Task SendMail(MimeMessage email)
        {
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail));

            var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
