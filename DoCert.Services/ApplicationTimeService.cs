using System.Runtime.InteropServices;
using Havit.Services.TimeServices;

namespace DoCert.Services;

/// <summary>
/// Provides current time in local time-zone ("Central Europe Standard Time", "Europe/Prague" for non-Windows platforms).
/// </summary>
public class ApplicationTimeService : TimeZoneTimeServiceBase, ITimeService
{
    /// <summary>
    /// Returns time-zone you want to treat as local ("Central Europe Standard Time", "Europe/Prague" for non-Windows platforms).
    /// </summary>
    protected override TimeZoneInfo CurrentTimeZone => TimeZoneInfo.FindSystemTimeZoneById(
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Central Europe Standard Time"
            : "Europe/Prague"); // MacOS
}
