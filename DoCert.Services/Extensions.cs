using System.Text;

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
}