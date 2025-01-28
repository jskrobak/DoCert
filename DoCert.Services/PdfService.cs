using DoCert.Model;
using DoCert.Services.Pdf;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DoCert.Services;

public class PdfService:IPdfService
{
    public PdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        FontManager.RegisterFontWithCustomName("Arial", Resources.GetManifestResourceStream("Resources.arial.ttf"));
    }
    
    public byte[] CreateCertificate(Certificate cert, Agenda agenda)
    {
        return new CertificateDocument(cert, agenda).GeneratePdf();
    }
}
