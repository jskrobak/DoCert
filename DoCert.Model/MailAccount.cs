namespace DoCert.Model;

public class MailAccount
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SenderEmail { get; set; }
    public bool UseSsl { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    public string ClearPassword { get; set; }
    public string Username { get; set; }
    public string Bcc { get; set; }
    
    public bool IsTest { get; set; } = true;
   
    
}