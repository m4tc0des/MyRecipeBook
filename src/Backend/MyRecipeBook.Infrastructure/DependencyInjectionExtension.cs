using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.DataAcess;
using MyRecipeBook.Infrastructure.Repositories;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.Security.Tokens.Access;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddPasswordHasher();

            services.AddRepositories();

            services.AddTokensHandlers(configuration);

            services.AddDbContext(configuration);
        }

        private void AddRepositories()
        {
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

            services.AddScoped<IUserReadOnlyRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private void AddTokensHandlers(IConfiguration configuration)
        {
            var expirationTimeInMinutes = configuration.GetValue<uint>("JWT:ExpirationTimeMinutes");

            var signingKey = configuration.GetValue<string>("JWT:SigningKey")!;

            services.AddScoped<IAccessTokenGenerator>(provider =>
            {
                return new JwtTokenHandler(expirationTimeInMinutes, signingKey);
            });
        }

        private void AddDbContext(IConfiguration configuration)
        {
            services.AddDbContext<MyRecipeBookDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection");
                options.UseMySQL(connectionString!);
            });

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options.AddMySql5()
                    .WithGlobalConnectionString(_ =>
                    {
                        var connectionString = configuration.GetConnectionString("DbConnection")!;

                        return connectionString;
                    })
                    .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure"))
                    .For.All();
            });
        }

        private void AddPasswordHasher()
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        }
    }
}
