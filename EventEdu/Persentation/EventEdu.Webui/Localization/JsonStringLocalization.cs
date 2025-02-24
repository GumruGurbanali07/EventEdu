using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Localization;

public class JsonStringLocalization(IDistributedCache cache) : IStringLocalizer
{
    private static string ResourceFilePath => "Resources/translations.json";
    private static string CurrentCultureName => Thread.CurrentThread.CurrentCulture.Name;

    public LocalizedString this[string name]
    {
        get
        {
            string value = GetStringByName(name);
            return new LocalizedString(name, value, value == null!);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            LocalizedString localizedValue = this[name];
            return !localizedValue.ResourceNotFound
                ? new LocalizedString(name, string.Format(localizedValue.Value, arguments), false)
                : localizedValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        using FileStream fileStream = new(ResourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(fileStream);
        using JsonDocument jsonDocument = JsonDocument.Parse(streamReader.ReadToEnd());

        foreach (JsonProperty property in jsonDocument.RootElement.EnumerateObject())
        {
            string key = property.Name;
            string value = property.Value.GetString()!;
            yield return new LocalizedString(key, value, false);
        }
    }

    private string GetStringByName(string name)
    {
        string cacheKey = GenerateCacheKey(name);
        string cachedValue = cache.GetString(cacheKey)!;

        if (!string.IsNullOrEmpty(cachedValue)) return cachedValue;

        string resourceValue = GetValueFromResourceFile(name);

        if (!string.IsNullOrEmpty(resourceValue)) cache.SetString(cacheKey, resourceValue);

        return resourceValue;
    }

    private string GetValueFromResourceFile(string key)
    {
        using FileStream fileStream = new(ResourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(fileStream);
        using JsonDocument jsonDocument = JsonDocument.Parse(streamReader.ReadToEnd());

        JsonElement.ArrayEnumerator translations = jsonDocument.RootElement.EnumerateArray();
        foreach (JsonElement item in translations)
        {
            if (!item.TryGetProperty("Key", out JsonElement keyElement) || keyElement.GetString() != key) continue;

            if (item.GetProperty("LocalizedValue").TryGetProperty(CurrentCultureName, out JsonElement localizedValue))
                return localizedValue.GetString()!;
        }

        return default!;
    }

    private string GenerateCacheKey(string name)
    {
        return $"locale_{CurrentCultureName}_{name}";
    }
}