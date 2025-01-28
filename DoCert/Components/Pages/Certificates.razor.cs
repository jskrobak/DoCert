using DoCert.Model;
using DoCert.Services;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Certificate = DoCert.Model.Certificate;

namespace DoCert.Components.Pages;

public partial class Certificates : ComponentBase
{
    [Inject] protected IHxMessengerService Messenger { get; set; }
    [Inject] protected IHxMessageBoxService MessageBox { get; set; }
    [Inject] protected IPdfService PdfService { get; set; }
    
    [Inject] protected CertificateSendService CertificateSendService { get; set; }
    
    private HashSet<Certificate> selectedCertificates = [];
    private Certificate currentCertificate;
    private CertificateFilter filterModel = new();
    private HxGrid<Certificate> gridComponent;
    private HxModal progressModal;
    private HxModal errorModal;
    private List<string> errors = new();
    private float progressValue;
    private HxDropdownItem btnEmailSelected;
    private IEnumerable<Donor> availableDonors;
    private HxModal certificateEditModal;
    private HxModal selectDonorModal;
    private int? selectedDonorId;
    
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        //btnEmailSelected.
        
        CertificateSendService.Started += async (sender, e) => await progressModal.ShowAsync();
        CertificateSendService.Finished += async (sender, e) => await progressModal.HideAsync();
        CertificateSendService.Progress += (sender, e) =>
        {
            progressValue = (float)e.Completed / (float)e.Count;
            StateHasChanged();
        };
        
        await UpdateAvailableDonors();
    }
    
    private async Task UpdateAvailableDonors()
    {
        availableDonors = await DataService.GetDonorsWithoutCertificate();
    }

    private async Task<GridDataProviderResult<Certificate>> GetGridData(GridDataProviderRequest<Certificate> request)
    {
        var response =
            await DataService.GetCertificatesDataFragmentAsync(filterModel, request, request.CancellationToken);
        return new GridDataProviderResult<Certificate>()
        {
            Data = response.Data,
            TotalCount = response.TotalCount
        };
    }

    

    private async Task HandleDeleteClick(Certificate cert)
    {
        await DataService.DeleteCertificateAsync(cert);
        await gridComponent.RefreshDataAsync();
    }

    private async Task HandleSelectedDataItemChanged()
    {
        
    }

    private async Task HandleSavePdfClick(Certificate certificate)
    {
        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new SaveDialogOptions()
        {
            Title = "Uložit Pdf soubor",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Filters = [new FileFilter { Name = "PDF", Extensions = ["pdf"] }],
            //Properties = [OpenDialogProperty.openFile]
        };

        var filePath = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, dialogOptions);
        if (string.IsNullOrEmpty(filePath))
            return;
        var agenda = await DataService.GetAgendaAsync();
        var data = PdfService.CreateCertificate(certificate, agenda);


        await File.WriteAllBytesAsync(filePath, data);
    }

    private async Task HandleEmailSelectedClicked()
    {
        if (selectedCertificates.Count == 0)
        {
            Messenger.AddError("Označte potvrzení, která chcete odeslat.");
            return;
        }

        var agenda = await DataService.GetAgendaAsync();

        var req = new MessageBoxRequest()
        {
            Title = $"Odeslat potvrzení",
            Text = agenda.MailAccount.IsTest
                ? "Vybraná potvrzení budou odeslána v testovacím režimu."
                : "Opravdu chcete odeslat vybraná potvrzení?",
            Buttons = MessageBoxButtons.OkCancel
        };
        
        if (agenda.MailAccount.IsTest) req.Title += " (TESTOVACÍ REŽIM)";


    if (await MessageBox.ShowAsync(req) == MessageBoxButtons.Cancel)
            return;
        
            

        var result =
            await CertificateSendService.SendCertificatesAsync(selectedCertificates,
                await DataService.GetAgendaAsync());
        
        selectedCertificates.Clear();
        await gridComponent.RefreshDataAsync();

        await progressModal.HideAsync();

        if(result.Succeeded > 0)
            Messenger.AddInformation($"Odeslané e-maily: {result.Succeeded}");
        
        if(result.Errors.Count > 0)
        {
            errors = result.Errors.Select(e => e.Message).ToList();
            await errorModal.ShowAsync();
        }
            
        
    }

    private async Task HandleImportClick()
    {
        try
        {
            await DataService.CalculateCertificates();
            await gridComponent.RefreshDataAsync();
            Messenger.AddInformation("Import proběhl úspěšně.");
        }
        catch (Exception ex)
        {
            Messenger.AddError($"Import se nezdařil. Chyba: {ex.Message}");
        }
    }

    private async Task HandlePdfSelectedClicked()
    {
        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new OpenDialogOptions()
        {
            Title = "Vyberte složku",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Properties = [OpenDialogProperty.openDirectory]
        };


        var filePath = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, dialogOptions);

        if (filePath == null)
            return;
        if (filePath.Length == 0)
            return;
        if (string.IsNullOrEmpty(filePath[0]))
            return;

        var agenda = await DataService.GetAgendaAsync();

        try
        {
            foreach (var cert in selectedCertificates)
            {
                var data = PdfService.CreateCertificate(cert, agenda);
                var fileName = Path.Combine(filePath[0], cert.CreateFileName());
                await File.WriteAllBytesAsync(fileName, data);
            }

            Messenger.AddInformation("Hotovo");

        }
        catch (Exception ex)
        {
            Messenger.AddError($"Chyba při ukládání souborů: {ex.Message}");
        }
    }

    private async Task HandleNewCertClicked()
    {
        await selectDonorModal.ShowAsync();
    }

    private async Task HandleDeleteSelected()
    {
        if (selectedCertificates.Count == 0)
        {
            Messenger.AddError("Označte potvrzení, která chcete smazat.");
            return;
        }
        try
        {
            await DataService.DeleteCertificatesAsync(selectedCertificates);
            await gridComponent.RefreshDataAsync();
            Messenger.AddInformation("Smazáno");
        }
        catch (Exception ex)
        {
            Messenger.AddError($"Chyba při mazání: {ex.Message}");
        }
    }
    
    

    private async Task HandleEditClick(Certificate cert)
    {
        currentCertificate = cert;
        await certificateEditModal.ShowAsync();
    }

    private async Task UpdateCertificate()
    {
        if(currentCertificate.Amount <= 0)
        {
            Messenger.AddError("Vyplňte částku.");
            return;
        }
        
        await DataService.SaveCertificateAsync(currentCertificate);
        
        await gridComponent.RefreshDataAsync();
        await certificateEditModal.HideAsync();
        await UpdateAvailableDonors();
    }

    private async Task InsertCertificate()
    {
        if (!selectedDonorId.HasValue)
        {
            Messenger.AddError("Vyberte dárce.");
            return;
        }

        currentCertificate = new Certificate
        {
            DonorId = selectedDonorId.Value,
            Donor = availableDonors.First(d => d.Id == selectedDonorId.Value)
        };
        
        await selectDonorModal.HideAsync();
        await certificateEditModal.ShowAsync();
    }
}