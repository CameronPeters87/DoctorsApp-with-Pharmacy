using Nexmo.Api;
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
                //string Email = "drjamesgovender@gmail.com";
                string Email = "cameronpeters87@gmail.com";
                bool EnableSSL = true;
                string Password = "Rendezvous78";
                //string Password = "";
                int PortNumber = 587;
                string SMTP_Address = "smtp.gmail.com";

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

                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Host = SMTP_Address; // stmpClient.gmail.com
                    smtpClient.Port = PortNumber;
                    smtpClient.EnableSsl = EnableSSL;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = credential;

                    smtpClient.Send(mail);
                }
            }
            catch (System.Exception e)
            {
            }

        }

        public static void SendSms(string toNumber, string body)
        {
            var number = toNumber.Substring(1);

            number = "27" + number;

            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = "0f48d10b",
                ApiSecret = "R0hfbIdGjgNcG9Aa"
            });

            var results = client.SMS.Send(request: new SMS.SMSRequest
            {
                from = "Dr J Govender",
                to = number,
                text = body
            });

        }
    }
}