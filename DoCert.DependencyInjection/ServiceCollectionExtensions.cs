using System.Runtime.CompilerServices;
using DoCert.DataLayer.Repositories;
using DoCert.Entity;
using DoCert.Services;
using Havit.Data.EntityFrameworkCore.Patterns.Caching;
using Havit.Data.EntityFrameworkCore.Patterns.DependencyInjection;
using Havit.Data.EntityFrameworkCore.Patterns.UnitOfWorks.EntityValidation;
using Havit.Extensions.DependencyInjection;
using Havit.Extensions.DependencyInjection.Abstractions;
using Havit.Services.Caching;
using Havit.Services.TimeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoCert.DependencyInjection;

public static class ServiceCollectionExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static IServiceCollection ConfigureForWebServer(this IServiceCollection services,
        IConfiguration configuration)
    {
        var installConfiguration = new InstallConfiguration
        {
            DatabaseConnectionString = configuration.GetConnectionString("Database"),
        };


        services.WithEntityPatternsInstaller()
            .AddEntityPatterns()
            //.AddLocalizationServices<Language>()
            .AddDbContext<DoCertDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlite(installConfiguration.DatabaseConnectionString);
            })
            .AddDataLayer(typeof(IDonateRepository).Assembly);
        //.AddLookupService<ICountryByIsoCodeLookupService, CountryByIsoCodeLookupService>();

        services.AddSingleton<IEntityValidator<object>, ValidatableObjectEntityValidator>();

        //services.AddTransient<IEntityCacheManager, EntityCacheManager>();
        services.AddSingleton<ITimeService, ApplicationTimeService>();
        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddSingleton(new MemoryCacheServiceOptions { UseCacheDependenciesSupport = false });

        services.AddByServiceAttribute(typeof(DataLayer.Repositories.DonateRepository).Assembly, [
            ServiceAttribute.DefaultProfile, "WebServer"
        ]);
        services.AddByServiceAttribute(typeof(Services.DataService).Assembly, [
            ServiceAttribute.DefaultProfile, "WebServer"
        ]);
        
        services.AddMemoryCache();
        
        services.AddTransient<IDataService, DataService>();

        return services;
    }

}