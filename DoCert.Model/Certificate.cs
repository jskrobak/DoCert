using System.ComponentModel.DataAnnotations;

namespace DoCert.Model;

public class Certificate
{
    public int Id { get; set; }
    public int DonorId { get; set; }
    
    [Required]
    public Donor Donor { get; set; }
    
    [Range(1, double.MaxValue)]
    public double Amount { get; set; }
    public DateTime? LastSentDate { get; set; }
    
}