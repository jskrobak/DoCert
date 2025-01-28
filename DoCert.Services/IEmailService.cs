using DoCert.Model;

namespace DoCert.Services;

public interface IEmailService
{
    Task SendCertificateAsync(MailAccount mailAccount, string email, byte[] certificatePdf, string subject, string body);
}