using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.RequestCode;

public interface IRequestPasswordRecoveryCodeUseCase
{
    Task Execute(RequestPasswordRecoveryJson request);
}
