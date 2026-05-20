using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using Shouldly;
using Xunit.Sdk;

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
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
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

    private RegisterUserUseCase CreateUseCase()
    {
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepository = new IUserReadOnlyRepositoryBuilder().Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var passwordHasher = new IPasswordHasherBuilder().Build();

        return new RegisterUserUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepository, unitOfWork);
    }
}
