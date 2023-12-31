﻿using System.Reflection;
using CommunityToolkit.Diagnostics;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Exodus.Api;

/// <summary>
/// Holding class for extension methods to <see cref="IServiceCollection"/> to add Exodus services and configurations.
/// </summary>
public static class ExodusDependencyInjectionExtensions
{
    /// <summary>
    /// Registers all of the Exodus infrastructure services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> to use</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained</returns>
    public static IServiceCollection AddExodus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAsyncInitialization();
        services.AutoRegister();
        services.AddDatabase(configuration);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddHttpContextAccessor();
        services.AddSingleton<IClock>(SystemClock.Instance);

        var jwtSecret = configuration["JwtSecret"];
        Guard.IsNotNullOrEmpty(jwtSecret);
        
        services.AddJWTBearerAuth(jwtSecret);
        services.AddAuthorization();
        services.AddFastEndpoints(o =>
        {
            o.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All);
        });

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.IsNotNullOrEmpty(connectionString);

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.UseNodaTime()));
    }
}