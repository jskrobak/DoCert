namespace DoCert.Components.Shared.ColorMode;

public class ColorModeClientResolver: IColorModeResolver
{
    public Havit.Blazor.Components.Web.Bootstrap.ColorMode GetColorMode()
    {
        return Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto; // client always resolves to auto, cookie used for server prerendering
    }
}