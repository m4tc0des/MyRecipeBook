using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;

namespace Validators.Testes.User.Register;

public class RegisterUserValidatorTests
{
    [Fact]
    public void Sucess() //AAA A = Arange A = Act A = Assert
    {
        var request = new RequestRegisterUserJson
        {
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Password = "password123"
        };

        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }
}
