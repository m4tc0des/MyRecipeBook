using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Repositories;

namespace UseCases.Tests.User.Register;

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();
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
