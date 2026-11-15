using EWallet.Api.Handlers;
using EWallet.Core.DepedencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Swagger ApiDocs
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// EF-Core Connection and Seeding
builder.Services.AddCorePersistence(builder.Configuration);

// Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Services
builder.Services.AddServices();

// Repositories
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
