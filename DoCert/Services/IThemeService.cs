namespace DoCert.Services;

public interface IThemeService
{
    Task<List<Theme>> GetAllThemesAsync();
    Task<Theme> GetSelectedThemeAsync();
    Task SetSelectedThemeAsync(Theme theme);
}