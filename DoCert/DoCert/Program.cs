using BitzArt.Blazor.Cookies;
using DoCert.Components;
using DoCert.Components.Shared.ColorMode;
using DoCert.DependencyInjection;
using DoCert.Services;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Data.EntityFrameworkCore.Patterns.DependencyInjection;
using Havit.Extensions.DependencyInjection;
using Havit.Services.Caching;
using SoloX.BlazorJsBlob;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddHxMessageBoxHost();
builder.Services.AddSingleton<IColorModeResolver, ColorModeClientResolver>();
builder.AddBlazorCookies();
builder.Services.AddHttpClient<IThemeService, ThemeService>();



builder.Services.ConfigureForWebServer(builder.Configuration);

builder.Services.AddJsBlob();
builder.Services.AddTransient<FakeDataService>();


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
    .AddAdditionalAssemblies(typeof(DoCert.Client._Imports).Assembly);

app.Run();
