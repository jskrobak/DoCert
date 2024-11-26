namespace DoCert.Model;

public class ImportProfile
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Encoding { get; set; }
    public string Delimiter { get; set; }
    public string CultureInfo { get; set; }
    public int? DonorNameColumnIndex { get; set; }
    public int? AmountColumnIndex { get; set; }
    public int? DateColumnIndex { get; set; }
    public int? AccountNumberColumnIndex { get; set; }
    public int? VariableSymbolColumnIndex { get; set; }
    public int? SpecificSymbolColumnIndex { get; set; }
    public int? ConstantSymbolColumnIndex { get; set; }
    public int? MessageColumnIndex { get; set; }
    public bool HasHeaderRecord { get; set; }
}