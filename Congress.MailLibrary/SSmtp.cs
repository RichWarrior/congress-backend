using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Congress.MailLibrary
{
    public class SSmtp : ISmtp
    {
        private string smtp_host { get; set; }
        private int smtp_port { get; set; }
        private string smtp_sender { get; set; }
        private string smtp_password { get; set; }
        public SSmtp(string smtp_host,int smtp_port,string smtp_sender,string smtp_password)
        {
            this.smtp_host = smtp_host;
            this.smtp_port = smtp_port;
            this.smtp_sender = smtp_sender;
            this.smtp_password = smtp_password;
        }

        public Task<bool> SendAsync(MailEntity mailEntity)
        {
            return Task.Run(() => Send(mailEntity));
        }

        public bool Send(MailEntity mailEntity)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.BodyEncoding = Encoding.UTF8;
                message.HeadersEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Subject = mailEntity.subject;
                message.From = new MailAddress(this.smtp_sender);
                message.Body = mailEntity.body;
                message.To.Add(mailEntity.to);
                SmtpClient smtp = new SmtpClient(this.smtp_host);
                smtp.Credentials = new NetworkCredential(this.smtp_sender, this.smtp_password);
                smtp.EnableSsl = true;
                smtp.Port = this.smtp_port;
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
