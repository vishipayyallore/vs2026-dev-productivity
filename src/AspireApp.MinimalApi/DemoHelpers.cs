using System.Security.Cryptography;

namespace Aspire.MinimalApi;

internal static class DemoHelpers
{
    internal static readonly string[] Summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    internal static int GetSecureRandomInt(int minValue, int maxValue)
    {
        return RandomNumberGenerator.GetInt32(minValue, maxValue);
    }


}
