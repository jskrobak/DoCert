using System.Reflection;

namespace DoCert.Services;

public static class Resources
{
    public static Stream GetManifestResourceStream(string resourceName)
    {
        var info = Assembly.GetExecutingAssembly().GetName();
        var name = info.Name;
        return Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream($"{name}.{resourceName}")!;

    }
}