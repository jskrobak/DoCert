namespace DoCert.Model;

public class Donor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? IBAN { get; set; }
    public string? VariableSymbol { get; set; }
    
    public int? CertificateId { get; set; }
    public Certificate? Certificate { get; set; }
}