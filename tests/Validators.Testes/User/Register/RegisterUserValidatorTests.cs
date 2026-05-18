using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
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

    [Fact]
    public void Validate_ShouldHaveError_When_NameIsEmpty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Name = string.Empty;

        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_EmailIsEmpty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Email = string.Empty;

        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_PasswordIsEmpty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Password = string.Empty;

        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_EmailIsInvalid()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        request.Email = "invalid-email";
        
        var validator = new RegisterUserValidator();
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
        
        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_INVALID));
        });
    }
}
