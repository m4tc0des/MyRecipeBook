using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using Shouldly;

namespace UseCases.Tests.User.Register;

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();

        result.Tokens.ShouldNotBeNull();

        result.Name.ShouldBe(request.Name);

        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();

        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(condition =>
        {
            condition.Count.ShouldBe(1);

            condition.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(condition =>
        {
            condition.Count.ShouldBe(1);

            condition.ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS);
        });
    }

    private RegisterUserUseCase CreateUseCase(string? emailThatAlreadyExists = null)
    {
        var accessTokenGenetatorBuilder = IAccessTokenGeneratorBuilder.Build();

        var unitOfWork = IUnitOfWorkBuilder.Build();

        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();

        var passwordHasher = new IPasswordHasherBuilder().Build();

        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();

        if (emailThatAlreadyExists.IsNotEmpty())
        {
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(emailThatAlreadyExists);
        }

        return new RegisterUserUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork, accessTokenGenetatorBuilder);
    }
}
