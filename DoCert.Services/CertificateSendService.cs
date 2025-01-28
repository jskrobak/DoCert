using DoCert.Model;
using Microsoft.Extensions.Logging;

namespace DoCert.Services;

public class CertificateSendService(ILogger<CertificateSendService> logger, 
    IEmailService emailService,
    IPdfService pdfService,
    IDataService dataService)
{
    public delegate void StartHandler(object sender, EventArgs e);
    public delegate void FinishHandler(object sender, EventArgs e);
    public delegate void ProgressHandler(object sender, ProgressEventArgs e);
    
    public event StartHandler Started;
    public event FinishHandler Finished;
    public event ProgressHandler Progress;

    public class ProgressEventArgs(int count, int completed) : EventArgs
    {
        public int Count { get; set; } = count;
        public int Completed { get; set; } = completed;
    }
    
    public class SendResult
    {
        public int Succeeded { get; set; }
        public List<Exception> Errors { get; set; } = [];
    }
    
    protected virtual void OnStarted(EventArgs e)
    {
        Started?.Invoke(this, e);
    }
    
    protected virtual void OnFinished(EventArgs e)
    {
        Finished?.Invoke(this, e);
    }
    
    protected virtual void OnProgress(ProgressEventArgs e)
    {
        Progress?.Invoke(this, e);
    }
    
    public async Task<SendResult> SendCertificatesAsync(HashSet<Certificate> certificates, Agenda agenda)
    {
        var count = certificates.Count;

        OnProgress(new ProgressEventArgs(count, 0));
        
        OnStarted(EventArgs.Empty);

        var result = new SendResult();
        
        var completed = 0;
        
        foreach (var cert in certificates)
        {
            try
            {
                await Task.Delay(100);
                
                if (string.IsNullOrEmpty(cert.Donor.Email))
                    throw new Exception($"{cert.Donor.Name} : chybí e-mail dárce.");

                var data = pdfService.CreateCertificate(cert, agenda);

                await emailService.SendCertificateAsync(agenda.MailAccount, cert.Donor.Email, data,
                    agenda.MailSubject, agenda.MailBody.Replace("@ORGANIZATION", agenda.Organization));

                result.Succeeded++;

                cert.LastSentDate = DateTime.Now;
                await dataService.SaveCertificateAsync(cert);
            }
            catch (Exception e)
            {
                result.Errors.Add(e);
            }

            completed++;
            OnProgress(new ProgressEventArgs(count, completed));
        }
        
        OnFinished(EventArgs.Empty);

        return result;
    }
}