namespace DoCert.Model;

public class Donate
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; }
    public int DonorId { get; set; }
    public Donor Donor { get; set; }
    public DateTime Date { get; set; }
    public string VariableSymbol { get; set; }
    public string SpecificSymbol { get; set; }
    public string ConstantSymbol { get; set; }
    public string Message { get; set; }

}