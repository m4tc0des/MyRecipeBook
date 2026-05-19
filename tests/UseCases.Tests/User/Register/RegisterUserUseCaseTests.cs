using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Repositories;
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
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    private RegisterUserUseCase CreateUseCase()
    {
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepository = new IUserReadOnlyRepositoryBuilder().Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var passwordHasher = new IPasswordHasherBuilder().Build();

        return new RegisterUserUseCase( passwordHasher, userWriteOnlyRepository, userReadOnlyRepository, unitOfWork);
    }
}
