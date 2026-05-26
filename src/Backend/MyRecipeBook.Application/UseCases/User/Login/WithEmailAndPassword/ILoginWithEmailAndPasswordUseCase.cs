using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
}
