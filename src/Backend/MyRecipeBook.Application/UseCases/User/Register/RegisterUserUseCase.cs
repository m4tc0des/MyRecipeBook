using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase
    {
        public ResponserRegisterUserJson Execute(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var results = validator.Validate(request);

            return new ResponserRegisterUserJson
            {
                Name = request.Name
            };
        }
    }
}
