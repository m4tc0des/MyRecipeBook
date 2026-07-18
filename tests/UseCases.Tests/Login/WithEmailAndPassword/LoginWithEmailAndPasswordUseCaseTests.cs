using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using Shouldly;

namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Fact]
    public async Task Sucess()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestLoginJsonBuilder.Build();

        request.Email = user.Email;

        var usecase = CreateUseCase(request.Password, user);

        var result = await usecase.Execute(request);

        result.ShouldNotBeNull();

        result.Tokens.ShouldNotBeNull();

        result.Name.ShouldBe(user.Name);

        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();

        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    [Fact]
    public async Task ShouldThrowException_When_UserDontExist()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessage =>
        {
            errorMessage.Count.ShouldBe(1);

            errorMessage.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    [Fact]
    public async Task ShouldThrowException_When_PasswordIsIncorrect()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestLoginJsonBuilder.Build();

        request.Email = user.Email;

        var useCase = CreateUseCase(user: user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessage =>
        {
            errorMessage.Count.ShouldBe(1);

            errorMessage.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, MyRecipeBook.Domain.Entities.User? user = null)
    {
        var accessTokenGenetatorBuilder = IAccessTokenGeneratorBuilder.Build();

        var passwordHasherBuilder = new IPasswordHasherBuilder();

        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();

        if (user is not null)
        {
            userReadOnlyRepositoryBuilder.GetByEmail(user);
        }

        if (password.IsNotEmpty())
        {
            passwordHasherBuilder.VerifyPassword(password);
        }

        return new LoginWithEmailAndPasswordUseCase(passwordHasherBuilder.Build(), userReadOnlyRepositoryBuilder.Build(), accessTokenGenetatorBuilder);
    }
}
