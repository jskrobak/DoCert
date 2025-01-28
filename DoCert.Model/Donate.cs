namespace DoCert.Model;

public class Donate
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public int BankAccountId { get; set; }
    public string? Iban { get; set; }
    public int DonorId { get; set; }
    public Donor Donor { get; set; }
    public DateTime Date { get; set; }
    public string? VariableSymbol { get; set; }
    public string? Message { get; set; }

}