using BitzArt.Blazor.Cookies;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddHxMessageBoxHost();
builder.AddBlazorCookies();

await builder.Build().RunAsync();
