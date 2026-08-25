using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Application.UseCases.Filter;

public class FilterRecipeUseCase : IFilterRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _recipeReadRepository;

    public FilterRecipeUseCase(ILoggedUser loggedUser, IRecipeReadOnlyRepository recipeReadRepository)
    {
        _loggedUser = loggedUser;
        _recipeReadRepository = recipeReadRepository;
    }
    public Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson? request)
    {
        throw new NotImplementedException();
    }
}
