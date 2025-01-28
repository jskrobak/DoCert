namespace DoCert.Model;

public class Agenda
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Organization { get; set; }
    public string RegNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string FooterText { get; set; }
    public string PlaceAndDateTemplate { get; set; }
    public string BodyTemplate { get; set; }
    public string MailSubject { get; set; }
    public string MailBody { get; set; }
    public string IssuerName { get; set; }
    public string IssuerPosition { get; set; }
    public byte[] LogoPng { get; set; }
    public byte[] StamperPng { get; set; }
    public int MailAccountId { get; set; }
    public MailAccount MailAccount { get; set; }
  
}