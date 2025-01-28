using NLog;
using NLog.Config;
using NLog.Targets;

namespace DoCert.DependencyInjection;

public static class NLog
{
    public static void Configure(string logDirPath, LogLevel logLevel)
    {
        Directory.CreateDirectory(logDirPath);
        var logFilePath = Path.Combine(logDirPath, $"{DateTime.Today:yyyyMMdd}.log");

        var config = new LoggingConfiguration();
        

        // Create a FileTarget
        var fileTarget = new FileTarget("fileTarget")
        {
            FileName = logFilePath,
            Layout = "${longdate} [${level:uppercase=true}] ${message} ${exception:format=tostring}",
            KeepFileOpen = true,
            ConcurrentWrites = false,
            Encoding = System.Text.Encoding.UTF8
        };
        
        var consoleTarget = new ColoredConsoleTarget("consoleTarget")
        {
            Layout = "${longdate} [${level:uppercase=true}] ${message} ${exception:format=tostring}"
        };

        // Add the target to the configuration
        config.AddTarget(fileTarget);
        config.AddTarget(consoleTarget);

        // Define a rule: everything from Info and above goes to file
        var rule = new LoggingRule("*", logLevel, fileTarget);
        config.LoggingRules.Add(rule);
        
        config.LoggingRules.Add(new LoggingRule("*", logLevel, consoleTarget));

        // Apply the config to NLog
        LogManager.Configuration = config;
    }
}