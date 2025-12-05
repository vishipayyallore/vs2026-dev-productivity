namespace AspireApp.SharedLib.DTOs;

/// <summary>
/// Data Transfer Object for Stock Price information
/// </summary>
public record StockPriceDto(
    string Symbol,
    decimal CurrentPrice,
    decimal DailyChange,
    decimal DailyChangePercent,
    DateTime LastUpdated);
