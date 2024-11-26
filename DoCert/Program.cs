using BitzArt.Blazor.Cookies;
using DoCert;
using DoCert.Components.Shared.ColorMode;
using DoCert.DataLayer.Repositories;
using DoCert.DependencyInjection;
using DoCert.Entity;
using DoCert.Services;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using SoloX.BlazorJsBlob;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Havit.Data.EntityFrameworkCore.Patterns.DependencyInjection;
using Havit.Data.EntityFrameworkCore.Patterns.UnitOfWorks.EntityValidation;
using Havit.Extensions.DependencyInjection;
using Havit.Extensions.DependencyInjection.Abstractions;
using Havit.Services.Caching;
using Havit.Services.TimeServices;
using Microsoft.EntityFrameworkCore;
using App = DoCert.Components.App;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var appSettingsService = new LocalAppSettingsService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DoCert"));

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseElectron(args);
//builder.WebHost.UseEnvironment("Development");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddHxMessageBoxHost();
builder.Services.AddSingleton<IAppSettingsService>(appSettingsService);
builder.Services.AddElectron();

builder.Services.ConfigureForWebServer(builder.Configuration, appSettingsService);

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
    app.UseHsts();
}

app.UseHttpsRedirection();

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
}

app.Run();
