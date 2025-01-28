using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using DoCert.Model;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace DoCert.Services;

public class EmailService(ILogger<EmailService> logger, IDataProtectionProvider dataProtectionProvider)
    : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector(Defaults.DataProtectorPurpose);

    public async Task SendCertificateAsync(MailAccount acc, string email, byte[] certificatePdf, string subject, string body)
    {
        var msg = new MailMessage()
        {
            Sender = new MailAddress(acc.SenderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
            BodyEncoding = Encoding.UTF8,
            To = { new MailAddress(email) },
            Attachments = { new Attachment(new MemoryStream(certificatePdf), "potvrzeni.pdf") }
        };

        await SendEmailAsync(acc, msg);
    }
    
    private async Task SendEmailAsync(MailAccount acc, MailMessage msg)
    {
        acc.ClearPassword = _protector.Unprotect(acc.Password);
        
        var recipients = msg.To.ToList();

        msg.To.Clear();

        msg.Headers.Add("X-Mailer", "artipa.MailTools");
        msg.From = new MailAddress(acc.SenderEmail);
        msg.ReplyToList.Add(acc.SenderEmail);
        msg.Sender = msg.From;

        // Send messages
        var mx = new SmtpClient();
        mx.UseDefaultCredentials = false;
        mx.Host = acc.Host;
        mx.EnableSsl = acc.UseSsl;
        mx.Port = acc.Port;
        mx.Credentials = new NetworkCredential(acc.Username, acc.ClearPassword);
            
        mx.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            
        if (acc.IsTest)
        {
            msg.To.Add(acc.SenderEmail);
                
            logger.LogInformation("Start send mail message. {0}", GetInfo(msg));
            await mx.SendMailAsync(msg);
        }
        else
        {
            foreach (var recipient in recipients)
            {
                msg.To.Clear();
                msg.To.Add(recipient);
                
                if(!string.IsNullOrWhiteSpace(acc.Bcc))
                    msg.Bcc.Add(acc.Bcc);
                    
                logger.LogInformation("Start send mail message. {0}", GetInfo(msg));
                await mx.SendMailAsync(msg);
            }
        }
    }

    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Cancelled)  
        {  
            logger.LogInformation("Send canceled.");  
        }  
        if (e.Error != null)  
        {  
            logger.LogInformation(e.Error.ToString());  
        }  
        else  
        {  
            logger.LogInformation("Email sent successfully");  
        }  
    }
    
    private string GetInfo(MailMessage msg)
    {
        return
            $"from={msg.From}, to={string.Join(",", msg.To.Select(m => m.Address))}, bcc={msg.Bcc} subject={msg.Subject}";
    }
}