using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggerUser _loggerUser;

    public GetUserProfileUseCase(ILoggerUser loggerUser)
    {
        _loggerUser = loggerUser;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var loggedUser = await _loggerUser.Get();

        return loggedUser.Adapt<ResponseUserProfileJson>();
    }
}
