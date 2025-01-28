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
    
    private HashSet<Donate> selectedDonates = [];
    private HxOffcanvas importCsvOffCanvasComponent;
    private Donate currentDonate;
    private DonateFilter filterModel = new();
    private HxGrid<Donate> gridComponent;

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

        throw new Exception("ycyxcxycy");
    }

    private async Task HandleImportCsv()
    {
        if (!HybridSupport.IsElectronActive)
        {
            await Import(@"c:\Data\Projects\private\DoCert\data\data.xlsx");
            return;
        }

        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new OpenDialogOptions()
        {
            Title = "Vyberte Xlsx soubor",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Filters = [new FileFilter { Name = "XLSX", Extensions = ["xlsx"] }],
            //Properties = [OpenDialogProperty.openFile]
        };

        var files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, dialogOptions);

        if (files == null) return;
        if (files.Length == 0) return;
        if (!File.Exists(files[0])) return;

        var filePath = files[0];
        
        await Import(filePath);
        
       
    }

    private async Task Import(string filePath)
    {
        try
        {
            await using var stream = File.OpenRead(filePath);
            await DataService.ImportDonatesFromExcelAsync(stream);
            await gridComponent.RefreshDataAsync();
            Messenger.AddInformation("Import proběhl úspěšně.");
        }
        catch (Exception ex)
        {
            Messenger.AddError($"Import se nezdařil. Chyba: {ex.Message}");
        }
        
        
        await importCsvOffCanvasComponent.HideAsync();
    }

    private async Task HandleDeleteSelected()
    {
        if (selectedDonates.Count == 0)
        {
            Messenger.AddError("Označte potvrzení, která chcete smazat.");
            return;
        }
        try
        {
            await DataService.DeleteDonatesAsync(selectedDonates);
            await gridComponent.RefreshDataAsync();
            Messenger.AddInformation("Smazáno");
        }
        catch (Exception ex)
        {
            Messenger.AddError($"Chyba při mazání: {ex.Message}");
        }
    }
}

