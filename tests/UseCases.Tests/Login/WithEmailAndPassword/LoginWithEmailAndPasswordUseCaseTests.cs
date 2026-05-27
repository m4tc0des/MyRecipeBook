using CommonTestUtilities.Repositories;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;

namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, MyRecipeBook.Domain.Entities.User? user = null )
    {
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();

        if (user is not null)
        {
            userReadOnlyRepositoryBuilder.GetByEmail(user);
        }

        if (password.IsNotEmpty())
        {
            passwordHasherBuilder.
        }

        return new LoginWithEmailAndPasswordUseCase(passwordHasherBuilder.Build(), userReadOnlyRepositoryBuilder.Build());
    }
}
