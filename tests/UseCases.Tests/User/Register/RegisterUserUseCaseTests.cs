using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;

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
        return new RegisterUserUseCase(null, null, null, null);
    }
}
