using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using Shouldly;

namespace Validators.Testes.User.Register;

public class RegisterUserValidatorTests
{
    [Fact]
    public void Success() //AAA A = Arange A = Act A = Assert
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }
}
