// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text;
using CSVFile;
using CsvHelper;
using CsvHelper.Configuration;

var filePath = @"c:\Data\Projects\private\DoCert\data\2980066193-0800_2024-01-01_2024-12-31 (1).csv";

var name = CultureInfo.CurrentCulture.Name;

var ci = CultureInfo.GetCultureInfo(name);

/*
using var sr = new StreamReader(filePath);
using var cr = new CSVReader(sr, new CSVSettings()
{
   //Encoding = Encoding.GetEncoding(1250),
    FieldDelimiter = ',',
    TextQualifier = '"',
});

await foreach (var line in cr.LinesAsync())
{
    Console.WriteLine(line.GetValue(0));
}
*/

var config = new CsvConfiguration(CultureInfo.CurrentCulture)
{
    HasHeaderRecord = true,
    Delimiter = ",",
};

using var reader = new StreamReader(filePath);
using var csv = new CsvReader(reader, config);
var header = csv.HeaderRecord;


while (csv.Read())
{
    Console.WriteLine(csv.Parser.Record);
}