using System.Text;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SoloX.BlazorJsBlob.Services.Impl;


namespace DoCert.Components.Pages;

public partial class Development : ComponentBase
{
    private async Task HandleFakeCsvDonatesClick()
    {
        var donates = FakeDataService.PrepareFakeDonates();
        using var stream = new MemoryStream();
        await using var tw = new StreamWriter(stream, Encoding.GetEncoding(1250));

        var i = 0;
        foreach(var d in donates)
        {
            if(i == 0)
                await tw.WriteLineAsync("Id; Name; Email; Birthdate; Amount; Date; AccountNumber; VariableSymbol; SpecificSymbol; ConstantSymbol; Message");

            var items = new List<string>()
            {
                d.Id.ToString(),
                d.Donor.Name,
                d.Donor.Email,
                d.Donor.Birthdate?.ToString("s"),
                d.Amount.ToString("N"),
                d.Date.ToString("s"),
                d.BankAccount.AccountNumber,
                d.VariableSymbol,
                d.SpecificSymbol,
                d.ConstantSymbol,
                d.Message
            };
            
            await tw.WriteLineAsync(string.Join(";", items));

            i++;
        }
        
        stream.Seek(0, SeekOrigin.Begin);

        if (HybridSupport.IsElectronActive)
        {
            var mainWindow = Electron.WindowManager.BrowserWindows.First();
            var saveOptions = new SaveDialogOptions
            {
                Title = "Save a CSV file",
                DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
                Filters = [new FileFilter { Name = "CSV", Extensions = ["csv"] }],
            };

            var path = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, saveOptions);

            if (string.IsNullOrEmpty(path))
                return;
            
            await File.WriteAllBytesAsync(path, stream.ToArray());

            await Electron.Shell.OpenExternalAsync("file://" + path);
            
            return;
        }

        await using var blob = await BlobService
            .CreateBlobAsync(stream, "plain/text");
        
        await BlobService.SaveAsFileAsync(blob, "donates.csv");
    }

    private async Task HandleSeedData()
    {
        var donates = FakeDataService.PrepareFakeDonates();
        await DataService.InsertDonatesAsync(donates);
    }
}