using System.Security.Principal;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace DoCert;

public class ElectronHelper
{
    public static void CreateMenu()
    {
        var fileMenu = new MenuItem[]
        {
            new MenuItem { Label = "Home", 
                Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/") },
            new MenuItem { Label = "Counter", 
                Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/Counter") },
            new MenuItem { Type = MenuType.separator },
            new MenuItem { Role = MenuRole.quit }
        };

        var viewMenu = new MenuItem[]
        {
            new MenuItem { Role = MenuRole.reload },
            new MenuItem { Role = MenuRole.forcereload },
            new MenuItem { Role = MenuRole.toggledevtools },
            new MenuItem { Type = MenuType.separator },
            new MenuItem { Role = MenuRole.resetzoom },
            new MenuItem { Role = MenuRole.zoomin },
            new MenuItem { Role = MenuRole.zoomout },
            new MenuItem { Type = MenuType.separator },
            new MenuItem { Role = MenuRole.togglefullscreen }
        };

        var menu = new MenuItem[] 
        {
            new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },
            new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu }
        };

        //Electron.NativeTheme.SetThemeSource(ThemeSourceMode.System);
        Electron.Menu.SetApplicationMenu([]);
    }

    public static async Task CreateElectronWindowAsync()
    {
        var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions()
        {
            WebPreferences = new WebPreferences()
            {
                NodeIntegration = false
            },
            TitleBarStyle = TitleBarStyle.customButtonsOnHover,
            //TitleBarOverlay = true,
        });
        
        /*
        window.OnClose += async (e) =>
        {
            e.PreventDefault();
            //await window.HideAsync();
        };
        */
        
        window.OnClosed += () =>
        {
            Electron.App.Quit();
        };
    } 
}