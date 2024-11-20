using System.Text.Json;
using BitzArt.Blazor.Cookies;

namespace DoCert.Services;

public class ThemeService(HttpClient httpClient, 
    ICookieService cookieService): IThemeService
{
    private List<Theme> _themes = [];

    public async Task<List<Theme>> GetAllThemesAsync()
    {

        if (_themes.Count != 0) return _themes;

        try
        {
            var result = await httpClient.GetStreamAsync("https://bootswatch.com/api/5.json");
            var themesHolder = await JsonSerializer.DeserializeAsync<ThemeHolder>(result,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            _themes = themesHolder.Themes;
        }
        catch
        {
            Console.WriteLine("Unable to fetch themes from Bootswatch API.");
            _themes = [];
        }

        _themes = _themes.Prepend(new Theme { Name = "Bootstrap5", CssCdn = "FULL_LINK_HARDCODED_IN_RAZOR" }).ToList();

        return _themes;
    }

    public async Task<Theme> GetSelectedThemeAsync()
    {
        var cookie = await cookieService.GetAsync(Theme.ThemeCookieName);
        var themes = await GetAllThemesAsync();
        return themes.FirstOrDefault(t => t.Name == "Cosmo");
    }
    
    public async Task SetSelectedThemeAsync(Theme theme)
    {
        await cookieService.SetAsync(Theme.ThemeCookieName, theme.Name, DateTimeOffset.MaxValue);
    }
}

public class ThemeHolder
{
    public List<Theme> Themes { get; set; }
}