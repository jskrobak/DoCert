using System.Globalization;
using DoCert;
using DoCert.DependencyInjection;
using DoCert.Entity;
using DoCert.Services;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using ElectronNET.API;
using NLog.Extensions.Logging;
using App = DoCert.Components.App;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("cs-CZ");

var appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DoCert");
var appSettingsService = new LocalAppSettingsService(appDataDir);

var logLevel = args.Contains("--debug") ? NLog.LogLevel.Debug : NLog.LogLevel.Info;
DoCert.DependencyInjection.NLog.Configure(Path.Combine(appDataDir, "logs"), logLevel);

//restore db if needed
new BackupService().Restore();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseElectron(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog(); 

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddHxMessageBoxHost();
builder.Services.AddSingleton<IAppSettingsService>(appSettingsService);
builder.Services.AddTransient<BackupService>();
builder.Services.AddElectron();
builder.Services.AddDataProtection();
builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddTransient<IPdfService, PdfService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<CertificateSendService>();

builder.Services.ConfigureForWebServer(appSettingsService);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    //.AddAdditionalAssemblies(typeof(DoCert.Client._Imports).Assembly);
    ;

if (HybridSupport.IsElectronActive)
{
    ElectronHelper.CreateMenu();
    await ElectronHelper.CreateElectronWindowAsync();
    Electron.App.Quitting += async () =>
    {
        await app.StopAsync();
    };
}

app.Run();




