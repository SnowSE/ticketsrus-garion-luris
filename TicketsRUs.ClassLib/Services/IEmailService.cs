using System.Net.Mail;

using MailKit.Net.Smtp;

using MimeKit;

using TicketsRUs.ClassLib.Data;

using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TicketsRUs.ClassLib.Services;

public interface IEmailService
{
    Task SendEmailAsync(string receiverEmail, string identifier);
}