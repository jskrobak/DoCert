using DoCert.Model;

namespace DoCert;

public static class Extensions
{
    public static string CreateFileName(this Certificate cert)
    {
        return $"potvrzeniodaru-{Normalize(cert.Donor.Name)}-{cert.Donor.Birthdate?.ToString("yyyyMMdd")}.pdf";
    }
    
    private static string Normalize(string text)
    {
        var stringFormD = text.Normalize(System.Text.NormalizationForm.FormD);
        var retVal = new System.Text.StringBuilder();
        foreach (var t in stringFormD.Where(t => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(t) != System.Globalization.UnicodeCategory.NonSpacingMark))
        {
            retVal.Append(t);
        }
        return retVal.ToString().Normalize(System.Text.NormalizationForm.FormC);
    }
}