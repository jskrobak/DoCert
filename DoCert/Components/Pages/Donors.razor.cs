using DoCert.Model;
using ElectronNET.API;
using ElectronNET.API.Entities;
using ExcelDataReader.Log;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;

namespace DoCert.Components.Pages;

public partial class Donors : ComponentBase
{
    [Inject] protected IHxMessengerService Messenger { get; set; }
    [Inject] protected IHxMessageBoxService MessageBox { get; set; }
    
    private HxOffcanvas importCsvOffCanvasComponent;
    private Donor currentDonor;
    private DonorFilter filterModel = new();
    private HxGrid<Donor> gridComponent;
    private HxModal donorEditModal;
    
    private async Task<GridDataProviderResult<Donor>> GetGridData(GridDataProviderRequest<Donor> request)
    {
        var response = await DataService.GetDonorsDataFragmentAsync(filterModel, request, request.CancellationToken);
        return new GridDataProviderResult<Donor>()
        {
            Data = response.Data,
            TotalCount = response.TotalCount
        };
    }
    
    private async Task HandleDeleteClick(Donor donor)
    {
        await DataService.DeleteDonorAsync(donor);
        await gridComponent.RefreshDataAsync();
    }
    
    private async Task HandleNewItemClicked()
    {
        currentDonor = new Donor();
        await donorEditModal.ShowAsync();
    }

    private async Task HandleSelectedDataItemChanged()
    {
        await donorEditModal.ShowAsync();
    }

    private async Task HandleNewCertClick(Donor donor)
    {
        var agenda = await DataService.GetAgendaAsync();
        
        await DataService.CreateCertificateAsync(donor, agenda.Year);
    }
    
    private async Task HandleImportCsvClicked()
    {
        await importCsvOffCanvasComponent.ShowAsync();
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
            //using var sr = new StreamReader(csvFilePath);}
            await using var stream = File.OpenRead(filePath);
            //await DataService.ImportDonorsFromCsvAsync(sr);
            await DataService.ImportDonorsFromExcelAsync(stream);
            await gridComponent.RefreshDataAsync();
            
            Messenger.AddInformation("Import proběhl úspěšně.");
        }
        catch (Exception ex)
        {
            Messenger.AddError($"Import se nezdařil. Chyba: {ex.Message}");
        }

        await importCsvOffCanvasComponent.HideAsync();
    }

    private Task HandleDeleteSelected()
    {
        throw new NotImplementedException();
    }

    private async Task SaveDonor()
    {
        await DataService.SaveDonorAsync(currentDonor);
        
        await gridComponent.RefreshDataAsync();
        await donorEditModal.HideAsync();
    }   

    private async Task HandleEditClick(Donor donor)
    {
        currentDonor = donor;
        await donorEditModal.ShowAsync();
    }
}