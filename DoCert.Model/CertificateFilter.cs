namespace DoCert.Model;

public class CertificateFilter
{
    public string? DonorName { get; set; }
    public List<int> Years { get; set; } = [];
}