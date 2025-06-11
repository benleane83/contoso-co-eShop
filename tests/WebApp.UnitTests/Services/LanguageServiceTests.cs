using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace eShop.WebApp.UnitTests.Services;

public class LanguageServiceTests
{
    private readonly ILogger<LanguageService> _logger = Substitute.For<ILogger<LanguageService>>();
    private readonly LanguageService _languageService;

    public LanguageServiceTests()
    {
        _languageService = new LanguageService(_logger);
    }

    [Fact]
    public void GetCurrentCulture_ReturnsCurrentCultureName()
    {
        // Arrange
        var originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        try
        {
            // Act
            var result = _languageService.GetCurrentCulture();

            // Assert
            Assert.Equal("en-US", result);
        }
        finally
        {
            // Restore original culture
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void GetSupportedCultures_ReturnsExpectedCultures()
    {
        // Act
        var cultures = _languageService.GetSupportedCultures().ToList();

        // Assert
        Assert.Equal(2, cultures.Count);
        Assert.Contains(cultures, c => c.Code == "en-US" && c.Name == "English" && c.Flag == "🇺🇸");
        Assert.Contains(cultures, c => c.Code == "ar-SA" && c.Name == "العربية" && c.Flag == "🇸🇦");
    }

    [Fact]
    public async Task SetCultureAsync_WithValidCulture_SetsCookie()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var culture = "ar-SA";

        // Act
        await _languageService.SetCultureAsync(culture, httpContext);

        // Assert
        Assert.True(httpContext.Response.Headers.ContainsKey("Set-Cookie"));
        var cookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
        Assert.Contains(CookieRequestCultureProvider.DefaultCookieName, cookieHeader);
        // The cookie value is URL-encoded, so we check for the encoded version
        Assert.Contains("c%3Dar-SA", cookieHeader);
    }

    [Fact]
    public async Task SetCultureAsync_WithInvalidCulture_LogsWarningAndDoesNotSetCookie()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var invalidCulture = "invalid-culture";

        // Act
        await _languageService.SetCultureAsync(invalidCulture, httpContext);

        // Assert
        Assert.False(httpContext.Response.Headers.ContainsKey("Set-Cookie"));
        
        // Verify warning was logged
        _logger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(v => v.ToString()!.Contains("Unsupported culture: invalid-culture")),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}