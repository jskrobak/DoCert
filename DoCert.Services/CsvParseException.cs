namespace DoCert.Services;

public class CsvParseException: Exception
{
    public CsvParseException(string message, int? index, string[] line, Exception innerException): base(message, innerException)
    {
        Index = index;
        Line = line;
    }
    public int? Index { get; set; }
    public string[] Line { get; set; }
}