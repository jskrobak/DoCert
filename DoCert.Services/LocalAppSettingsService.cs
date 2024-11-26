using System.Text.Json;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DoCert.Services;

public class LocalAppSettingsService: IAppSettingsService
{
    
    private string _appSettingsFilePath;
    private string _dbFilePath;
    
    public AppSettings Settings { get; protected set; }
    public string DatabaseConnectionString { get; protected set; }

    public LocalAppSettingsService(string dir)
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        _appSettingsFilePath = Path.Combine(dir, "appsettings.json");

        if (File.Exists(_appSettingsFilePath))
        {
            Settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_appSettingsFilePath));
        }
        else
        {
            Settings = new AppSettings
            {
                ColorMode = ColorMode.Auto,
                SideBarCollapsed = false
            };

            SaveSettingsAsync().GetAwaiter().GetResult();
        }

        _dbFilePath = Path.Combine(dir, "docert.db");
        if (!File.Exists(_dbFilePath))
            File.Copy("Assets/docert.db", _dbFilePath);

        DatabaseConnectionString = $"Data Source={_dbFilePath}";
    }


    public async Task SaveSettingsAsync()
    {
        var json = JsonSerializer.Serialize(Settings);
        await File.WriteAllTextAsync(_appSettingsFilePath, json);
    }

}