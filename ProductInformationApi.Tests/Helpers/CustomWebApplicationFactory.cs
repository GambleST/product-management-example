using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductInformationApi.Contexts;

namespace ProductInformationApi.Tests.Helpers;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // Dedicated Test database
    private readonly string _connectionString = "DataSource=:memory:";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ProductInformationDbContext>>();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddDbContext<ProductInformationDbContext>(options => { options.UseSqlite(connection); });

            // Ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductInformationDbContext>();
            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");
    }
}