using MailKit.Security;
using MimeKit;
using PSOENotificaciones.Contexto;
using System;
using System.Configuration;
using System.IO;

namespace PSOENotificaciones.Helpers
{
    public static class Utils
    {
        public static void SendMail(string mail, string subject, string body)
        {
            try
            {
                ParametrosInternos pi = new ParametrosInternos();
                ParametrosInternos parametro = pi.GetParametroInternoPorId((int)ParametroInterno.EnvioEmails);

                if (parametro.Valor == "1")
                {
                    string emailOutlook = ConfigurationManager.AppSettings["emailOutlook"];
                    string passOutlook = ConfigurationManager.AppSettings["passOutlook"];

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("PSOE", emailOutlook));
                    message.To.Add(new MailboxAddress("nombre", mail));
                    message.Subject = subject;
                    message.Body = new TextPart("html") { Text = body };

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                        client.Authenticate(emailOutlook, passOutlook);
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void SendMailAdjunto(string mail, string subject, string body, string path)
        {
            try
            {
                ParametrosInternos pi = new ParametrosInternos();
                ParametrosInternos parametro = pi.GetParametroInternoPorId((int)ParametroInterno.EnvioEmails);

                if (parametro.Valor == "1")
                {
                    string emailOutlook = ConfigurationManager.AppSettings["emailOutlook"];
                    string passOutlook = ConfigurationManager.AppSettings["passOutlook"];

                    MimeMessage message = new MimeMessage();
                    message.From.Add(new MailboxAddress("PSOE", emailOutlook));
                    message.To.Add(new MailboxAddress("nombre", mail));
                    message.Subject = subject;
                    //message.Body = new TextPart("html") { Text = body };

                    var body2 = new TextPart("html")
                    {
                        Text = body
                    };

                    var attachment = new MimePart("image", "gif")
                    {
                        Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(path)
                    };

                    // now create the multipart/mixed container to hold the message text and the
                    // image attachment
                    var multipart = new Multipart("mixed")
                    {
                        body2,
                        attachment
                    };

                    message.Body = multipart;

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                        client.Authenticate(emailOutlook, passOutlook);
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }
    }
}