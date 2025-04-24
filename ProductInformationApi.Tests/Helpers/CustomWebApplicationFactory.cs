using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductInformationApi.Contexts;
using Testcontainers.MySql;

namespace ProductInformationApi.Tests.Helpers;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Dedicated Test database
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithDatabase("productdb")
        .WithUsername("mainuser")
        .WithPassword("supersecurepassword")
        .Build();

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ProductInformationDbContext>>();
            services.AddDbContext<ProductInformationDbContext>(options =>
            {
                options.UseMySql(
                    _mySqlContainer.GetConnectionString(),
                    new MySqlServerVersion(new Version(8, 0, 34))
                );
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductInformationDbContext>();
            db.Database.Migrate();
        });

        builder.UseEnvironment("Development");
    }
}