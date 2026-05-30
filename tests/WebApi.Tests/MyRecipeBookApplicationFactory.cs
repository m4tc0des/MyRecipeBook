using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAcess;
using Testcontainers.MySql;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer;
    public MyRecipeBookApplicationFactory()
    {
        //var mysql = new MySQLContainer(DockerImageName.parse("mysql:5.7.34"));
        //mysql.start();
        _mySqlContainer = new MySqlBuilder("mysql:8.0")
            .WithDatabase("meulivrodereceitas")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _mySqlContainer.GetConnectionString()
                };

                configuration.AddInMemoryCollection(parameters);
            });
    }
    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();

        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var (user, password) = UserBuilder.Build();

        user.Password = passwordHasher.HashPassword(password);

        await dbContext.Users.AddAsync(user);

        await dbContext.SaveChangesAsync();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _mySqlContainer.StopAsync();
    }
}
