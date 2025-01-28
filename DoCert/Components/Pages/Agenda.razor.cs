using System.Diagnostics;
using System.Text;
using DoCert.Entity;
using DoCert.Model;
using DoCert.Services;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Certificate = DoCert.Model.Certificate;

namespace DoCert.Components.Pages;

public partial class Agenda : ComponentBase
{
    [Inject] protected IHxMessengerService Messenger { get; set; }
    [Inject] protected IDataProtectionProvider DataProtectionProvider { get; set; }
    [Inject] protected IDataService DataService { get; set; }
    [Inject] protected IPdfService PdfService { get; set; }
    [Inject] protected BackupService BackupService { get; set; }

    protected IDataProtector protector => DataProtectionProvider.CreateProtector(Defaults.DataProtectorPurpose);
    protected Model.Agenda agenda;
    protected Model.MailAccount mailAccount;

    protected string logoPngBase64;
    protected string stamperPngBase64;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        mailAccount = await DataService.GetMailAccountAsync();
        if (mailAccount == null)
        {
            mailAccount = new Model.MailAccount()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseSsl = true,
                Name = "Gmail",
                Username = "@gmail.com",
                SenderEmail = "@gmail.com",
                Bcc = "",
                Password = protector.Protect("password")
            };

            await DataService.InsertMailAccountAsync(mailAccount);
        }

        try
        {
            mailAccount.ClearPassword = protector.Unprotect(mailAccount.Password);
        }
        catch
        {
            mailAccount.ClearPassword = "";
        }

        agenda = await DataService.GetAgendaAsync();
        if (agenda == null)
        {
            agenda = new Model.Agenda()
            {
                BodyTemplate =
                    "Potvrzujeme, že @DONOR-NAME, datum narození @DONOR-BIRTH-DATE, poskytl/a na bezúplatném plnění, peněžních darech, našemu sboru Církve bratrské v průběhu roku @YEAR částku @AMOUNT Kč, (slovy: @AMOUNT-IN-WORDS). Částka byla použita na náboženské a charitativní účely. Toto potvrzení se vydává na vlastní žádost pro účel odpočtu ze základu daně z příjmu.",
                MailSubject = "Potvrzení o daru pro daňové účely",
                MailBody =
                    "Vážený dárci,\n\nv příloze Vám zasíláme potvrzení o daru pro daňové účely.\n\nS pozdravem\n\n@ORGANIZATION",
                MailAccount = mailAccount,
                Address = "",
                Organization = "",
                FooterText = "",
                IssuerName = "",
                IssuerPosition = "",
                PlaceAndDateTemplate = "V Praze dne @DATE",
                LogoPng = [],
                StamperPng = []

            };
            await DataService.InsertAgendaAsync(agenda);
        }

        logoPngBase64 = agenda.LogoPng != null
            ? $"data:image/png;base64, {Convert.ToBase64String(agenda.LogoPng)}"
            : "data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==";

        if(agenda.StamperPng != null)
            stamperPngBase64 = $"data:image/png;base64, {Convert.ToBase64String(agenda.StamperPng)}";
    }


    private async Task SaveAgenda()
    {
        try
        {
            await DataService.UpdateAgendaAsync(agenda, CancellationToken.None);
            Messenger.AddInformation("Nastavení bylo uloženo.");
        }
        catch (Exception e)
        {
            Messenger.AddError($"Uložení se nezdařilo. Chyba: {e.Message}");
        }
    }

    private async Task LoadLogoFileClicked()
    {
        if(Electron.WindowManager.BrowserWindows.Count == 0)
        {
            await SaveLogoFile(@"c:\Data\Projects\private\DoCert\data\CB-logo-symbol-cerna.png");
            return;
        }
        
        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new OpenDialogOptions()
        {
            Title = "Vyberte PNG soubor",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Filters = [new FileFilter { Name = "PNG", Extensions = ["png"] }],
            //Properties = [OpenDialogProperty.openFile]
        };

        var files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, dialogOptions);

        if (files == null) return;
        if (files.Length == 0) return;
        if (!File.Exists(files[0])) return;

        await SaveLogoFile(files[0]);

    }

    private async Task SaveLogoFile(string filePath)
    {
        try
        {
            agenda.LogoPng = await File.ReadAllBytesAsync(filePath);
            await DataService.SaveAgendaAsync(agenda, CancellationToken.None);
            Messenger.AddInformation("Logo bylo uloženo.");
        }
        catch (Exception e)
        {
            Messenger.AddError($"Uložení se nezdařilo. Chyba: {e.Message}");
        }
    }

    private async Task SaveMailAccount()
    {
        try
        {
            mailAccount.Password = protector.Protect(mailAccount.ClearPassword);
            await DataService.UpdateMailAccountAsync(mailAccount, CancellationToken.None);
            Messenger.AddInformation("Nastavení e-mailu bylo uloženo.");
        }
        catch (Exception e)
        {
            Messenger.AddError($"Uložení se nezdařilo. Chyba: {e.Message}");
        }
    }

    private async Task HandleTestReportClick()
    {
        if (Electron.WindowManager.BrowserWindows.Count == 0)
        {
            await TestPdf(@"test.pdf");
            return;
        }
            
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
        
        await TestPdf(@"test.pdf");
    }

    private async Task TestPdf(string filePath)
    {
        try
        {
            var agenda = await DataService.GetAgendaAsync();
            var data = PdfService.CreateCertificate(new Certificate()
            {
                Amount = 10000,
                Donor = new Donor()
                {
                    Name = "Jan Novák",
                    Birthdate = new DateTime(1980, 1, 1),
                }
            }, agenda);

            await File.WriteAllBytesAsync(filePath, data);
        }
        catch (Exception e)
        {
            Messenger.AddError(e.Message);
            return;
        }
        
        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
    }

    private async Task HandleBackupClick()
    {
        if (Electron.WindowManager.BrowserWindows.Count == 0)
        {
            BackupService.Backup("backup.zip");
            return;
        }
     
        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new SaveDialogOptions()
        {
            Title = "Vyberte ZIP soubor",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Filters = [new FileFilter { Name = "ZIP", Extensions = ["zip"] }],
            //Properties = [OpenDialogProperty.openFile]
        };

        var file = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, dialogOptions);

        if (file == null) return;
        
        BackupService.Backup(file);
        
        Messenger.AddInformation("Záloha byla uložena.");
    }

    private async Task HandleRestoreClick()
    {
        if (Electron.WindowManager.BrowserWindows.Count == 0)
        {
            BackupService.PrepareRestore("backup.zip");
            return;
        }
            
        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new OpenDialogOptions()
        {
            Title = "Vyberte ZIP soubor",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Filters = [new FileFilter { Name = "ZIP", Extensions = ["zip"] }],
            //Properties = [OpenDialogProperty.openFile]
        };

        var files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, dialogOptions);

        if (files == null) return;
        if (files.Length == 0) return;
        if (!File.Exists(files[0])) return;

        BackupService.PrepareRestore(files[0]);
        
        Electron.App.Relaunch();
    }

    private async Task LoadStamperFileClicked()
    {
        if(Electron.WindowManager.BrowserWindows.Count == 0)
        {
            await SaveStamperFile(@"c:\Data\Projects\private\DoCert\data\CBPraha13StamperAndSignature.png");
            return;
        }
        
        var mainWindow = Electron.WindowManager.BrowserWindows.First();
        var dialogOptions = new OpenDialogOptions()
        {
            Title = "Vyberte PNG soubor",
            DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
            Filters = [new FileFilter { Name = "PNG", Extensions = ["png"] }],
            //Properties = [OpenDialogProperty.openFile]
        };

        var files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, dialogOptions);

        if (files == null) return;
        if (files.Length == 0) return;
        if (!File.Exists(files[0])) return;

        await SaveStamperFile(files[0]);

    }

    private async Task SaveStamperFile(string filePath)
    {
        try
        {
            agenda.StamperPng = await File.ReadAllBytesAsync(filePath);
            await DataService.SaveAgendaAsync(agenda, CancellationToken.None);
            Messenger.AddInformation("Razítko bylo uloženo.");
        }
        catch (Exception e)
        {
            Messenger.AddError($"Uložení se nezdařilo. Chyba: {e.Message}");
        }
       
        
        
    }
}