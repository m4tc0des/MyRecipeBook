using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public GetRecipeByIdUseCase(IRecipeReadOnlyRepository recipeReadOnlyRepository, ILoggedUser loggedUser)
    {
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRecipeJson> Execute(Guid recipeId)
    {
        var recipe = await _recipeReadOnlyRepository.GetById(recipeId, _loggedUser.GetUserId());

        if (recipe is null)
        {
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        }

        return recipe.Adapt<ResponseRecipeJson>();
    }
}
