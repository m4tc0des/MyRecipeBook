using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase
    {
        public void Execute(RequestRegisterUserJson request)
        {
            ValidateAndThrowOnFailures(request);

            var user = request.Adapt<Domain.Entities.User>();

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
