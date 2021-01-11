using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using VALib.Configuration;


namespace VALib.Web
{
    public class Mail
    {
        //public static MailMessage getMailMessage(string[] MailTo, string Subject, Exception Body, MailAttachment[] Attachments = null)
        public static MailMessage getMailMessage(string[] MailTo, string Subject, string Body, MailAttachment[] Attachments = null)
        {
            
            MailMessage Mail = new MailMessage();
            Mail.From = new MailAddress(Settings.MailFrom);
            foreach (string t in MailTo) if (t.Trim().Length > 0) Mail.To.Add(new MailAddress(t));
            Mail.Subject = String.Format("VA Portale - {0}", Subject);
           
            Mail.Body = Body;
            Mail.IsBodyHtml = true;
            foreach (MailAttachment ma in Attachments ?? new MailAttachment[] { })
                Mail.Attachments.Add(new Attachment(ma.ContentStream, ma.Name, ma.MediaType));
            return Mail;
        }

        public static void Send(MailMessage msg)
        {            
            SmtpClient Smtp = new SmtpClient(Settings.MailSmtpServer);
            Smtp.Port = Settings.MailSmtpServerPort;
            Smtp.EnableSsl = false;            
            Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;            
            Smtp.Send(msg);
            Smtp.Dispose();
        }

        public class MailAttachment
        {
            public Stream ContentStream { get; set; }
            public String Name { get; set; }
            public String MediaType { get; set; }
        }
    }
}