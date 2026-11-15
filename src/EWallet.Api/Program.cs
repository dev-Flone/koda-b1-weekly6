using EWallet.Api.Handlers;
using EWallet.Core.Data;
using EWallet.Core.Interfaces.Services;
using EWallet.Core.Seeding;
using EWallet.Core.Services;
using Microsoft.EntityFrameworkCore;
using EWallet.Core.DepedencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Swagger ApiDocs
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// EF-Core
builder.Services.AddDbContext<AppDbContext>(options => options
    // Build Connection String
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("EWallet.Core"))
    // Seeding Data with UseSeeding and UseSeedingAsync Method
    .UseSeeding((context, _) =>
        {
            UserSeedData.Seed(context);
            WalletSeedData.Seed(context);
            PaymentMethodSeedData.Seed(context);
            TransactionSeedData.Seed(context);
        }
    )
    .UseAsyncSeeding(async (context, _, ct) =>
        {
            await UserSeedData.SeedAsync(context, ct);
            await WalletSeedData.SeedAsync(context, ct);
            await PaymentMethodSeedData.SeedAsync(context, ct);
            await TransactionSeedData.SeedAsync(context, ct);
        }
    )
    );

// Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// DI Repositories
// Inject All Repositories
builder.Services.AddRepositories();


var app = builder.Build();

// Database initialization
// To Actually run the SeedData
await app.Services.MigrateDbAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
