using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase: IRegisterUserUseCase
    {

        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserUseCase(IPasswordHasher passwordHasher)
        {

            _passwordHasher = passwordHasher;
        }

        public void Execute(RequestRegisterUserJson request)
        {
            ValidateAndThrowOnFailures(request);

            var user = request.Adapt<Domain.Entities.User>();

            user.Password = _passwordHasher.HashPassword(request.Password);
        }

        public void ValidateAndThrowOnFailures(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
