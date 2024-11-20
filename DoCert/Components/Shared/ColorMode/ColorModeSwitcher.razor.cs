using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DoCert.Components.Shared.ColorMode;

public partial class ColorModeSwitcher : ComponentBase, IDisposable
{
    [Inject] protected IColorModeResolver ColorModeResolver { get; set; }
	[Inject] protected PersistentComponentState PersistentComponentState { get; set; }
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private PersistingComponentStateSubscription _persistingSubscription;
	private IJSObjectReference _jsModule;
	private Havit.Blazor.Components.Web.Bootstrap.ColorMode _mode = Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto;

	protected override void OnInitialized()
	{
		_persistingSubscription = PersistentComponentState.RegisterOnPersisting(PersistMode);

		_mode = PersistentComponentState.TryTakeFromJson<Havit.Blazor.Components.Web.Bootstrap.ColorMode>("ColorMode", out var restored)
			? restored
			: ColorModeResolver.GetColorMode();
	}

	private Task PersistMode()
	{
		PersistentComponentState.PersistAsJson("ColorMode", _mode);
		return Task.CompletedTask;
	}

	private async Task HandleClick()
	{
		_mode = _mode switch
		{
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto,
			_ => Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto // fallback
		};

		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("setColorMode", _mode.ToString("g").ToLowerInvariant());
	}

	private async Task EnsureJsModule()
	{
		_jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Shared/ColorMode/ColorModeSwitcher.razor.js");
	}

	private IconBase GetIcon()
	{
		return _mode switch
		{
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto => BootstrapIcon.CircleHalf,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light => BootstrapIcon.Sun,
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark => BootstrapIcon.Moon,
			_ => throw new InvalidOperationException($"Unknown color mode '{_mode}'.")
		};
	}

	private string GetTooltip()
	{
		return _mode switch
		{
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Auto => "Auto color mode (theme). Click to switch to Dark.",
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Dark => "Dark color mode (theme). Click to switch to Light.",
			Havit.Blazor.Components.Web.Bootstrap.ColorMode.Light => "Light color mode (theme). Click to switch to Auto.",
			_ => "Click to switch color mode (theme) to Auto." // fallback
		};
	}

	void IDisposable.Dispose()
	{
		_persistingSubscription.Dispose();
	}
}