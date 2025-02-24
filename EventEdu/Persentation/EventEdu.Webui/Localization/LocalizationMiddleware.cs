using System.Collections.Concurrent;
using System.Globalization;

namespace EventEdu.Webui.Localization;

public class LocalizationMiddleware(RequestDelegate next)
{
    private static readonly HashSet<string> AvailableCultures = GetAllCultureNames();
    private static readonly ConcurrentDictionary<string, CultureInfo> CultureCache = [];

    public async Task InvokeAsync(HttpContext context)
    {
        string cultureKey = context.Session.GetString("lang") ?? "az-AZ";

        string? preferredCulture = ParseAcceptLanguageHeader(cultureKey);

        if (!string.IsNullOrWhiteSpace(preferredCulture) && DoesCultureExist(preferredCulture))
        {
            CultureInfo culture = CultureCache.GetOrAdd(preferredCulture, _ => new CultureInfo(preferredCulture));

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        await next(context);
    }

    private static bool DoesCultureExist(string cultureKey)
    {
        return AvailableCultures.Contains(cultureKey);
    }

    private static HashSet<string> GetAllCultureNames()
    {
        return CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Select(culture => culture.Name.ToLowerInvariant())
            .ToHashSet();
    }

    private static string? ParseAcceptLanguageHeader(string? headerValue)
    {
        IEnumerable<string>? items = headerValue?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split(';').First().Trim());

        return items?.FirstOrDefault()?.ToLowerInvariant();
    }
}