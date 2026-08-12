using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.DeleteById;

public class DeleteRecipeByIdUseCase: IDeleteRecipeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;

    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;

    public DeleteRecipeByIdUseCase(IRecipeWriteOnlyRepository recipeWriteOnlyRepository, ILoggedUser loggedUser)
    {
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(Guid recipeId)
    {
        var deleted = await _recipeWriteOnlyRepository.DeleteById(recipeId, _loggedUser.GetUserId());
        if (deleted == false)
        {
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        }
    }
}
