using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace eShop.WebApp.Services;

public interface ILanguageService
{
    string GetCurrentCulture();
    Task SetCultureAsync(string culture, HttpContext httpContext);
    IEnumerable<CultureOption> GetSupportedCultures();
}

public class LanguageService : ILanguageService
{
    private readonly ILogger<LanguageService> _logger;
    
    private static readonly CultureOption[] SupportedCultures =
    [
        new("en-US", "🇺🇸", "English"),
        new("ar-SA", "🇸🇦", "العربية")
    ];

    public LanguageService(ILogger<LanguageService> logger)
    {
        _logger = logger;
    }

    public string GetCurrentCulture()
    {
        return CultureInfo.CurrentCulture.Name;
    }

    public Task SetCultureAsync(string culture, HttpContext httpContext)
    {
        if (!SupportedCultures.Any(c => c.Code == culture))
        {
            _logger.LogWarning("Unsupported culture: {Culture}", culture);
            return Task.CompletedTask;
        }

        // Set culture cookie
        httpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax
            });

        _logger.LogInformation("Culture changed to: {Culture}", culture);
        return Task.CompletedTask;
    }

    public IEnumerable<CultureOption> GetSupportedCultures()
    {
        return SupportedCultures;
    }
}

public record CultureOption(string Code, string Flag, string Name);