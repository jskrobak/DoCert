using DoCert.Services;
using Microsoft.AspNetCore.Components;

namespace DoCert.Components.Layout;

public partial class Sidebar : ComponentBase
{
    [Inject] protected IAppSettingsService AppSettingsService { get; set; }
    
    private async Task HandleCollapsedChanged(bool arg)
    {
        AppSettingsService.Settings.SideBarCollapsed = arg;
        
        await AppSettingsService.SaveSettingsAsync();
    }
}