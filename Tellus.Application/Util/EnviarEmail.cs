using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Util
{
    public class EnviarEmail
    {
        public EnviarEmail(SmtpClient smtp, IConfiguration configuration)
        {
            _smtp = smtp;
            _configuration = configuration;
        }

        private SmtpClient _smtp;
        private IConfiguration _configuration;

        public bool Send(string to, string subject, string body, string[]? files, string bcc = "", string from = "")
        {

            try
            {
                MailMessage msg = new();
                body = body.Replace("\\r\\n", "\\n").Replace("\\n", "<br />");
                msg.From = new MailAddress(String.IsNullOrEmpty(from) ? _configuration.GetSection("Email:Username").Value : from);
                msg.Body = body;
                msg.Subject = subject;
                msg.To.Add(to);
                if (!String.IsNullOrEmpty(bcc))
                    msg.Bcc.Add(bcc);

                msg.IsBodyHtml = true;
                if (files != null)
                    foreach (var file in files)
                    {
                        if (!String.IsNullOrEmpty(file))
                        {
                            Attachment attachment = new(file);
                            msg.Attachments.Add(attachment);
                        }
                    }
                _smtp.Send(msg);
                _smtp.Dispose();
                msg.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
