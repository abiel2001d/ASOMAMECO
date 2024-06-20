using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using Infraestructure.Utils;

namespace Infraestructura.Utils
{
    class Correo
    {
        private readonly string _toEmail;
        private readonly string _subject;
        private readonly string _body;

        public Correo(string toEmail, string subject, string body)
        {
            _toEmail = toEmail;
            _subject = subject;
            _body = body;
        }

        public async Task EnviarCorreo()
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("ASOMAMECO Soporte", "asomamecosoporte@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", _toEmail));
            emailMessage.Subject = _subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = _body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("asomamecosoporte@gmail.com", "ehfc ywyz joqa nzrv");

                    await client.SendAsync(emailMessage);
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    string mensaje = "";
                    Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                    throw new Exception("Failed to send email", ex);
                }
            }
        }
    }
}
