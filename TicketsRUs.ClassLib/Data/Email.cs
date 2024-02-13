using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TicketsRUs.WebApp.Data
{
    public class Email
    {
        public static string sendEmail(string SenderEmail,
                             string SenderPass,
                             string ReceiverEmail)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Auto Emailer", SenderEmail));
                message.To.Add(new MailboxAddress("An Email in need of a Message", ReceiverEmail));
                message.Subject = "Automated Message System";

                message.Body = new TextPart("plain")
                {
                    Text = @"Thank you for your purchase,
 
                   We are very excited that you can come to the concert! We are very exited!
 
                    -- TicketUR"
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(SenderEmail, SenderPass);

                    client.Send(message);
                    client.Disconnect(true);
                }
                return "Email Sent";
            }
            catch (Exception e)
            {
                return "Bad Exception Happend";
            }
        }

    }
}
