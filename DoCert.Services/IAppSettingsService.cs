using Havit.Blazor.Components.Web.Bootstrap;

namespace DoCert.Services;

public interface IAppSettingsService
{
    AppSettings Settings { get; }
    string DatabaseConnectionString { get;  }
    Task SaveSettingsAsync();
}