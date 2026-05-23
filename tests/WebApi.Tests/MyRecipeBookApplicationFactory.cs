using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MySql;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlBuilderContainer;
    public MyRecipeBookApplicationFactory()
    {
        //var mysql = new MySQLContainer(DockerImageName.parse("mysql:5.7.34"));
        //mysql.start();
        _mySqlBuilderContainer = new MySqlBuilder("mysql:8.0")
            .WithDatabase("meulivrodereceitas")
            .Build();

    }

    public async Task InitializeAsync()
    {
        await _mySqlBuilderContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests");
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _mySqlBuilderContainer.StopAsync();
    }
}
