using Microsoft.EntityFrameworkCore;
using ProductInformationApi.Contexts;
using ProductInformationApi.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<ProductInformationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34)) // or whatever version you’ll use in Docker
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Automatically migrate database to make it easier for you guys - definitely not recommended for production!
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ProductInformationDbContext>();
    db.Database.Migrate();
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Required for WebApplicationFactory
public partial class Program
{
}