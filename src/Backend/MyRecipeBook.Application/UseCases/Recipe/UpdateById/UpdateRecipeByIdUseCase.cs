using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.UpdateById;

public class UpdateRecipeByIdUseCase : IUpdateRecipeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRecipeByIdUseCase(ILoggedUser loggedUser, IRecipeUpdateOnlyRepository recipeRepository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid recipeId, RequestRecipeJson request)
    {
        ValidateAndThrowOnFailures(request);

        var recipe = await _recipeRepository.GetById(recipeId, _loggedUser.GetUserId());

        if (recipe is null)
        {
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        }

        request.Adapt(recipe);

        await _unitOfWork.Commit();
    }

    public static void ValidateAndThrowOnFailures(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
        }
    }
}
