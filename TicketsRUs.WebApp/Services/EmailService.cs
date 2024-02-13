using MimeKit;
using MailKit.Net;
using TicketsRUs.ClassLib.Services;
using MailKit.Net.Smtp;

namespace TicketsRUs.WebApp.Services
{
    public class EmailService(IConfiguration config) : IEmailService
    {
        
        public async Task SendEmailAsync(string ReceiverEmail)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Auto Emailer", config["googleAccount"]));
                message.To.Add(new MailboxAddress("An Email in need of a Message", ReceiverEmail));
                message.Subject = "Automated Message System";

                message.Body = new TextPart("plain")
                {
                    Text = @"Thank you for your purchase,
 
               We are very excited that you can come to the concert! We are very exited!
 
                -- TicketUR"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(config["googleAccount"], config["googlePassword"]);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Bad Exception Happend");
            }
        }
    }
}
