using Microsoft.Extensions.Logging;

namespace AspireApp.MinimalApi;

/// <summary>
/// Logger message delegates for improved performance (CA1848)
/// </summary>
public static partial class LogMessages
{
    [LoggerMessage(LogLevel.Information, "[DevOnly] Resolved ConnectionStrings:productdb = {Conn}")]
    public static partial void LogConnectionStringResolved(ILogger logger, string conn);

    [LoggerMessage(LogLevel.Information, "[DevOnly] No ConnectionStrings:productdb found in configuration")]
    public static partial void LogConnectionStringNotFound(ILogger logger);

    [LoggerMessage(LogLevel.Warning, "[DevOnly] Failed to resolve/mask productdb connection string")]
    public static partial void LogConnectionStringError(ILogger logger, Exception ex);
}