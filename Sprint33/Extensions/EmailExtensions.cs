using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Sprint33.Extensions
{
    public static class EmailExtensions
    {
        public static void SendMail(string toEmail, string subject, string body)
        {
            try
            {
                string Email = ConfigurationManager.AppSettings["from"];
                bool EnableSSL = true;
                string Password = ConfigurationManager.AppSettings["password"];
                int PortNumber = int.Parse(ConfigurationManager.AppSettings["port"]);
                string SMTP_Address = ConfigurationManager.AppSettings["host"];

                MailMessage mail = new MailMessage();
                // From me
                mail.From = new MailAddress(Email);

                // To
                mail.To.Add(toEmail);
                mail.Subject = subject;  // Mail Subject  
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = Email,
                        Password = Password
                    };

                    smtpClient.Credentials = credential;
                    smtpClient.Host = SMTP_Address; // stmpClient.gmail.com
                    smtpClient.Port = PortNumber;
                    smtpClient.EnableSsl = EnableSSL;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(mail);
                }
            }
            catch (System.Exception e)
            {

                throw e;
            }

        }
    }
}