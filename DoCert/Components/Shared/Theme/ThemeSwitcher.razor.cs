using DoCert.Services;
using Microsoft.AspNetCore.Components;

namespace DoCert.Components.Shared.Theme;

public partial class ThemeSwitcher : ComponentBase
{
    [Inject] protected IThemeService ThemeService { get; set; }

    private List<DoCert.Theme> _themes = [];
    private DoCert.Theme _theme;
    
    protected override async Task OnInitializedAsync()
    {
        
        _themes = await ThemeService.GetAllThemesAsync();

        //_persistingSubscription = PersistentComponentState.RegisterOnPersisting(PersistTheme);

        _theme = await ThemeService.GetSelectedThemeAsync();
    }

}