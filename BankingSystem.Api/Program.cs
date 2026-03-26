using BankingSystem.Application;
using BankingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/banking-.log", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// .NET 10: Native OpenAPI Support (version 10.0.5)
builder.Services.AddOpenApi();

// .NET 10: Entity Framework Core 10 (version 10.0.0)
builder.Services.AddDbContext<BankingDbContext>(options => options.UseInMemoryDatabase("BankingDb"));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<AccountService>();

// .NET 10: Enhanced Health Checks (version 10.0.0)
builder.Services.AddHealthChecks().AddDbContextCheck<BankingDbContext>();

var app = builder.Build();
using (var scope = app.Services.CreateScope()) { scope.ServiceProvider.GetRequiredService<BankingDbContext>().Database.EnsureCreated(); }

if (app.Environment.IsDevelopment()) 
{ 
    // .NET 10: Native OpenAPI endpoint
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

// .NET 10: Health Checks endpoint
app.MapHealthChecks("/health");

app.MapGet("/", () => Results.Ok(new 
{ 
    Service = "Banking System", 
    Version = ".NET 10"
}));

Log.Information("Banking System starting...");
app.Run();
