using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using DoCert.Model;

namespace DoCert.Services;

public static class Extensions
{
    public static string ExportCsv<T>(this List<T> genericList)
    {
        var sb = new StringBuilder();
        var info = typeof(T).GetProperties();
        var header = typeof(T).GetProperties().Aggregate("", (current, prop) => current + (prop.Name + "; "));
        header = header.Substring(0, header.Length - 2);
            sb.AppendLine(header);
         
        foreach (var obj in genericList)
        {
            sb = new StringBuilder();
            var line = info.Aggregate("", (current, prop) => current + (prop.GetValue(obj, null) + "; "));
            line = line.Substring(0, line.Length - 2);
            sb.AppendLine(line);
        }

        return sb.ToString();
    }

    public static string GetString(this string[] array, int? index)
    {
        if(!index.HasValue)
            return string.Empty;
        
        if (index.Value < 0 || index.Value >= array.Length)
            return string.Empty;
        
        return array[index.Value];
    }
    
    public static decimal GetDecimal(this string[] array, int? index, CultureInfo cultureInfo)
    {
        try
        {
            return decimal.Parse(array.GetString(index), cultureInfo);    
        }
        catch (Exception ex)
        {
            throw new CsvParseException($"Error parsing decimal value at index {index}", index, array, ex);
        }
        
    }
    
    public static double GetDouble(this string[] array, int? index, CultureInfo cultureInfo)
    {
        try
        {
            return double.Parse(array.GetString(index), cultureInfo);    
        }
        catch (Exception ex)
        {
            throw new CsvParseException($"Error parsing decimal value at index {index}", index, array, ex);
        }
        
    }
    
    public static DateTime GetDateTime(this string[] array, int? index, CultureInfo cultureInfo)
    {
        try
        {
            return DateTime.Parse(array.GetString(index), cultureInfo);    
        }
        catch (Exception ex)
        {
            throw new CsvParseException($"Error parsing DateTime value at index {index}", index, array, ex);
        }
        
    }
    
    public static CsvConfiguration GetCsvConfiguration(this ImportProfile profile)
    {
        return new CsvConfiguration(CultureInfo.GetCultureInfo(profile.CultureInfo))
        {
            HasHeaderRecord = profile.HasHeaderRecord,
            Delimiter = profile.Delimiter,
            Encoding = Encoding.GetEncoding(profile.Encoding),
        };
    }
}