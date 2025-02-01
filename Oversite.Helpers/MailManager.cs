using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Oversite.Helpers
{
    public class MailManager
    {
        public void SendMail(string subject, String mailData, string mailTo)
        {
            try
            {
                using (SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.office365.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("vehiclefinance@manappuram.com", "VHef@998"),
                    TargetName = "STARTTLS/smtp.office365.com",
                    EnableSsl = true
                })
                {

                    MailMessage message = new MailMessage()
                    {
                        From = new MailAddress("vehiclefinance@manappuram.com "),
                        //Subject = "test mail",
                        Subject = subject,
                        IsBodyHtml = true,
                        Body = mailData,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8,

                    };
                    message.To.Add(new MailAddress(mailTo));
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
