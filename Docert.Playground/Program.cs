using ExcelDataReader;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

using var stream = File.OpenRead(@"c:\Data\Projects\private\DoCert\data\data.xlsx");

using var reader = ExcelReaderFactory.CreateReader(stream);

//sheet 0
reader.Read();
while (reader.Read())
{
    var name = reader.GetString(0);
    DateTime? dateOfBirth = reader.IsDBNull(1) ? null : reader.GetDateTime(1);
    var email = reader.GetString(2);
    var iban = reader.GetString(3);
    var vs = reader.GetValue(4).ToString();
}