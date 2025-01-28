using System.Runtime.CompilerServices;
using DoCert.DataLayer.Repositories;
using DoCert.Entity;
using DoCert.Model;
using DoCert.Services;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Patterns.DependencyInjection;
using Havit.Data.EntityFrameworkCore.Patterns.Infrastructure;
using Havit.Data.EntityFrameworkCore.Patterns.UnitOfWorks.EntityValidation;
using Havit.Data.Patterns.Infrastructure;
using Havit.Data.Patterns.Repositories;
using Havit.Services.Caching;
using Havit.Services.TimeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DoCert.DependencyInjection;

public static class ServiceCollectionExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static IServiceCollection ConfigureForWebServer(this IServiceCollection services, IAppSettingsService appSettingsService)
    {
        services.AddDbContext<IDbContext, DoCertDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlite(appSettingsService.DatabaseConnectionString);
        });

        services.AddDataLayerCoreServices();
        
        services.TryAddScoped<IAgendaRepository, AgendaRepository>();
        services.TryAddScoped<IRepository<Agenda>>(sp => sp.GetRequiredService<IAgendaRepository>());
        
        services.TryAddScoped<IDonorRepository, DonorRepository>();
        services.TryAddScoped<IRepository<Donor>>(sp => sp.GetRequiredService<IDonorRepository>());
        
        services.TryAddScoped<IDonateRepository, DonateRepository>();
        services.TryAddScoped<IRepository<Donate>>(sp => sp.GetRequiredService<IDonateRepository>());
        
        services.TryAddScoped<ICertificateRepository, CertificateRepository>();
        services.TryAddScoped<IRepository<Certificate>>(sp => sp.GetRequiredService<ICertificateRepository>());
        
        services.TryAddScoped<IMailAccountRepository, MailAccountRepository>();
        services.TryAddScoped<IRepository<MailAccount>>(sp => sp.GetRequiredService<IMailAccountRepository>());

        
        services.TryAddTransient<IEntityKeyAccessor<Agenda, int>, DbEntityKeyAccessor<Agenda, int>>();
        services.TryAddTransient<IEntityKeyAccessor<Donor, int>, DbEntityKeyAccessor<Donor, int>>();
        services.TryAddTransient<IEntityKeyAccessor<Donate, int>, DbEntityKeyAccessor<Donate, int>>();
        services.TryAddTransient<IEntityKeyAccessor<Certificate, int>, DbEntityKeyAccessor<Certificate, int>>();
        services.TryAddTransient<IEntityKeyAccessor<MailAccount, int>, DbEntityKeyAccessor<MailAccount, int>>();

        
        /*
        services.WithEntityPatternsInstaller()
            .AddEntityPatterns()
            //.AddLocalizationServices<Language>()
            .AddDbContext<DoCertDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlite(appSettingsService.DatabaseConnectionString);
            })
            .AddDataLayer(typeof(IDonateRepository).Assembly);
        */
        
        services.AddSingleton<IEntityValidator<object>, ValidatableObjectEntityValidator>();

        services.AddSingleton<ITimeService, ApplicationTimeService>();
        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddSingleton(new MemoryCacheServiceOptions { UseCacheDependenciesSupport = false });

        services.AddMemoryCache();

        return services;
    }
    
    

}