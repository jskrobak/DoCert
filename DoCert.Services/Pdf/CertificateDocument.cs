using DoCert.Model;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DoCert.Services.Pdf;

public class CertificateDocument(Certificate cert, Agenda agenda): IDocument
{
    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.DefaultTextStyle(Typography.Default);

                page.Margin(50);

                page.Header().Element(ComposeHeader);

                page.Content().Element(ComposeContent);

                page.Footer().Element(ComposeFooter);
            });
    }

    private void ComposeContent(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(15).Bold();
        
        container.PaddingBottom(30).Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().PaddingBottom(20).AlignCenter().Text("Potvrzení o výši darů").Style(titleStyle);
            });
            column.Item().Row(row =>
            {
                row.RelativeItem().AlignLeft().Text(FormatBodyText()).LineHeight(1.5f);
            });
            column.Item().Row(row =>
            {
                row.RelativeItem().AlignRight().Height(130).Image(agenda.StamperPng);
            });
            column.Item().Row(row =>
            {
                row.RelativeItem().AlignRight().Text(agenda.IssuerName);
                
            });
            column.Item().Row(row =>
            {
                row.RelativeItem().AlignLeft().Text(agenda.PlaceAndDateTemplate.Replace("@DATE", DateTime.Today.ToString("d")));
                row.RelativeItem().AlignRight().Text(agenda.IssuerPosition);
                
            });
        });
    }

    private string FormatBodyText()
    {
        var bodyText = agenda.BodyTemplate.Replace("@DONOR-NAME", cert.Donor.Name);
        bodyText = bodyText.Replace("@DONOR-BIRTH-DATE", cert.Donor.Birthdate.HasValue ? cert.Donor.Birthdate.Value.ToString("d") : "_____________");
        bodyText = bodyText.Replace("@YEAR", DateTime.Today.AddYears(-1).Year.ToString());
        bodyText = bodyText.Replace("@AMOUNT-IN-WORDS", Convert.ToInt32(cert.Amount).ToWords());
        bodyText = bodyText.Replace("@AMOUNT", cert.Amount.ToString("0.00"));
        return bodyText;
    }

    private void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().AlignLeft().Text(agenda.FooterText);
            });

        });
    }

    void ComposeHeader(IContainer container)
    {
        //var titleStyle = TextStyle.Default.FontSize(15).SemiBold().FontColor(Colors.Blue.Medium);
        var titleStyle = TextStyle.Default.FontSize(15).Bold();
        
        container.PaddingBottom(30).Row(row =>
        {
            if (agenda.LogoPng.Length > 0)
            {
                var logo = Image.FromBinaryData(agenda.LogoPng);
                //row.ConstantItem(100).Height(50).Placeholder();
                row.ConstantItem(100).Height(90).Image(logo);    
            }
            
            row.RelativeItem().PaddingTop(10).Column(column =>
            {
                column.Item().Text(agenda.Organization).Style(titleStyle);
                column.Item().AlignLeft().Text(agenda.Address);
                column.Item().AlignLeft().Text($"IČ: {agenda.RegNumber}");
                column.Item().AlignLeft().Text($"Tel: {agenda.PhoneNumber}");
                column.Item().AlignLeft().Text($"E-mail: {agenda.Email}");
                        
            });
            
            
        });
        
    }

}