using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using TicketsRUs.ClassLib.Data;

namespace TicketsRUs.ClassLib.Services
{
    public interface  IEmailService
    {
<<<<<<< HEAD
        Task SendEmailAsync(string receiverEmail, string identifier);
=======
        Task SendEmailAsync( string receiverEmail, string identifier);
>>>>>>> c55a7690086f51676d1cbbddb0fb82dea60db62a
    }
}
