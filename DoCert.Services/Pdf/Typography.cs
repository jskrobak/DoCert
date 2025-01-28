using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DoCert.Services.Pdf;

public static class Typography
{
    public static TextStyle Default => TextStyle.Default.FontFamily("Arial").FontSize(11);
}