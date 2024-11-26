using DoCert.Model;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;

namespace DoCert.Components.Pages;

public partial class Donates : ComponentBase
{
    [Inject] protected IHxMessengerService Messenger { get; set; }
    [Inject] protected IHxMessageBoxService MessageBox { get; set; }
    
    private HxOffcanvas importCsvOffCanvasComponent;
    private Donate currentDonate;
    private DonateFilter filterModel = new();
    private HxGrid<Donate> gridComponent;
    private ImportProfile importProfile;
    private List<ImportProfile> importProfiles;
    private string csvFilePath;

    private async Task<GridDataProviderResult<Donate>> GetGridData(GridDataProviderRequest<Donate> request)
    {
        var response = await DataService.GetDonatesDataFragmentAsync(filterModel, request, request.CancellationToken);
        return new GridDataProviderResult<Donate>()
        {
            Data = response.Data,
            TotalCount = response.TotalCount
        };
    }

    private async Task HandleDeleteClick(Donate donate)
    {
        await DataService.DeleteDonateAsync(donate);
        await gridComponent.RefreshDataAsync();
    }

    private async Task HandleImportCsvClicked()
    {
        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new OpenDialogOptions()
        {
            Title = "Vyberte CSV soubor",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Filters = [new FileFilter { Name = "CSV", Extensions = ["csv"] }],
            //Properties = [OpenDialogProperty.openFile]
        };

        var files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, dialogOptions);

        if (files == null) return;
        if (files.Length == 0) return;
        if (!File.Exists(files[0])) return;

        csvFilePath = files[0];
        
        importProfiles = await DataService.GetImportProfilesAsync();
        
        await importCsvOffCanvasComponent.ShowAsync();
    }

    private async Task HandleSelectedDataItemChanged()
    {
        // open or navigate to employee detail here (currentEmployee is set)
        // await dataItemEditComponent.ShowAsync();
        
    }

    private async Task HandleNewItemClicked()
    {
        await MessageBox.ShowAsync("Info", "This is the text", MessageBoxButtons.OkCancel);
        
        currentDonate = new Donate();
        // open or navigate to employee detail here
        // await dataItemEditComponent.ShowAsync();
        
    }

    private async Task HandleImportCsv()
    {
        using var sr = new StreamReader(csvFilePath);
        await DataService.ImportDonatesFromCsvAsync(sr, importProfile);
        
        await gridComponent.RefreshDataAsync();
        
        await importCsvOffCanvasComponent.HideAsync();
    }
}

