using EWallet.Core.Data;
using EWallet.Core.Interfaces.Services;
using EWallet.Core.Repositories;
using EWallet.Core.Repositories.Interfaces;
using EWallet.Core.Seeding;
using EWallet.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EWallet.Core.DepedencyInjection;

public static class DepedencyInjection
{
    // Method Helper

    // EFCore
    public static IServiceCollection AddCorePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Connection
        var connectionString = configuration.GetConnectionString("EWalletDb");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString)

                    // Data Seeding
                    .UseSeeding((context, _) => DISeed(context))
                    .UseAsyncSeeding((context, _, ct) => DISeedAsync(context, ct));
        });
        return services;
    }

    // Add Repositories
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITopUpRequestRepository, TopUpRequestRepository>();
        services.AddScoped<IWalletMutationRepository, WalletMutationRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        return services;
    }

    // Add Services
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }

    // Async Method

    // EFCore MigrationsScope
    public static async Task MigrateDbAsync(this IServiceProvider service)
    {
        using var scope = service.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }

    public static void DISeed(DbContext context)
    {
        UserSeedData.Seed(context);
        WalletSeedData.Seed(context);
        PaymentMethodSeedData.Seed(context);
        TransactionSeedData.Seed(context);
    }

    public static async Task DISeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        await UserSeedData.SeedAsync(context, cancellationToken);
        await WalletSeedData.SeedAsync(context, cancellationToken);
        await PaymentMethodSeedData.SeedAsync(context, cancellationToken);
        await TransactionSeedData.SeedAsync(context, cancellationToken);
    }
}