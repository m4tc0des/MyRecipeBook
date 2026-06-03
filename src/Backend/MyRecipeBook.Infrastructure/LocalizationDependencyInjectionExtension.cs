using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace MyRecipeBook.Infrastructure;

public static class LocalizationDependencyInjectionExtension
{
    public static IServiceCollection AddConfiguredLocalization(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo> { new("en"), new("pt-BR"), new("es") };

            options.DefaultRequestCulture = new RequestCulture("en");

            options.SupportedCultures = supportedCultures;

            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
        });

        return services;
    }

    public static IApplicationBuilder UseConfiguredLocalization(this IApplicationBuilder app)
    {
        var localizationOptions = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();

        app.UseRequestLocalization(localizationOptions.Value);

        return app;
    }
}
