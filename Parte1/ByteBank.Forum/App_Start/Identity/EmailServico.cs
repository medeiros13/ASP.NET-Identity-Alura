using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace ByteBank.Forum.App_Start.Identity
{
    public class EmailServico : IIdentityMessageService
    {
        private readonly string EMAIL_ORIGEM = ConfigurationManager.AppSettings["emailServico:email_remetente"];
        private readonly string EMAIL_SENHA = ConfigurationManager.AppSettings["emailServico:email_senha"];

        public async Task SendAsync(IdentityMessage message)
        {
            using (var mensagemDeEmail = new MailMessage())
            {
                mensagemDeEmail.From = new MailAddress(EMAIL_ORIGEM);

                mensagemDeEmail.Subject = message.Subject;
                mensagemDeEmail.To.Add(message.Destination);
                mensagemDeEmail.Body = message.Body;

                //SMTP = Simple Mail Transport Protocol
                using (var SMTPClient = new SmtpClient())
                {
                    SMTPClient.UseDefaultCredentials = true;
                    SMTPClient.Credentials = new NetworkCredential(EMAIL_ORIGEM, EMAIL_SENHA);

                    SMTPClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SMTPClient.Host = "smtp.gmail.com";
                    SMTPClient.Port = 587;
                    SMTPClient.EnableSsl = true;
                    SMTPClient.Timeout = 20_000;

                    await SMTPClient.SendMailAsync(mensagemDeEmail);
                }
            }
        }
    }
}