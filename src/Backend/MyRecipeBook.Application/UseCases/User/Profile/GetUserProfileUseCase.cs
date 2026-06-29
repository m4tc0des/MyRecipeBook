using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggerUser;

    public GetUserProfileUseCase(ILoggedUser loggerUser)
    {
        _loggerUser = loggerUser;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var loggedUser = await _loggerUser.Get();

        return loggedUser.Adapt<ResponseUserProfileJson>();
    }
}
