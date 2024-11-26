using DoCert.Services;
using Microsoft.AspNetCore.Components;

namespace DoCert.Components.Layout;

public partial class MainLayout
{
    private const string TitleBase = "DoCert";
    private const string TitleSeparator = " | ";

    [Inject] protected NavigationManager NavigationManager { get; set; }
    
    private string _title;
    
    protected override void OnParametersSet()
    {
        var path = new Uri(NavigationManager.Uri).AbsolutePath.TrimEnd('/');

        var lastSegmentStart = path.LastIndexOf("/");
        if (lastSegmentStart > 0)
        {
            _title = path[(lastSegmentStart + 1)..] + TitleSeparator + TitleBase;
            return;
        }

        _title = "DoCert";
    }
}