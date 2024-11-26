using DoCert.Services;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DoCert.Components.Shared.ColorMode;

public partial class ColorModeSwitcher : ComponentBase
{
    [Inject] protected IAppSettingsService AppSettingsService { get; set; }
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);
		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("setColorMode", AppSettingsService.Settings.ColorMode.ToString("g").ToLowerInvariant());
	}

	private async Task HandleClick()
	{
		AppSettingsService.Settings.ColorMode = AppSettingsService.Settings.ColorMode switch
		{
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto,
			_ => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto // fallback
		};

		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("setColorMode", AppSettingsService.Settings.ColorMode.ToString("g").ToLowerInvariant());
		
		await AppSettingsService.SaveSettingsAsync();
	}

	private async Task EnsureJsModule()
	{
		_jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Shared/ColorMode/ColorModeSwitcher.razor.js");
	}

	private IconBase GetIcon()
	{
		 return AppSettingsService.Settings.ColorMode switch
		{
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto => BootstrapIcon.CircleHalf,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light => BootstrapIcon.Sun,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark => BootstrapIcon.Moon,
			_ => throw new InvalidOperationException($"Unknown color mode '{AppSettingsService.Settings.ColorMode}'.")
		};
	}

	private string GetTooltip()
	{
		return AppSettingsService.Settings.ColorMode switch
		{
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto => "Auto color mode (theme). Click to switch to Dark.",
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark => "Dark color mode (theme). Click to switch to Light.",
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light => "Light color mode (theme). Click to switch to Auto.",
			_ => "Click to switch color mode (theme) to Auto." // fallback
		};
	}
}