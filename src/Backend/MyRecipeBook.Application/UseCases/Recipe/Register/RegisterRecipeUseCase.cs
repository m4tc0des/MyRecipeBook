using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRecipeUseCase(IRecipeWriteOnlyRepository repository, ILoggedUser loggedUser, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisterRecipeJson> Execute(RequestRecipeJson request)
    {
        Validate(request);

        var recipe = request.Adapt<MyRecipeBook.Domain.Entities.Recipe>();

        recipe.UserId = _loggedUser.GetUserId();

        await _repository.Add(recipe);

        await _unitOfWork.Commit();

        return new ResponseRegisterRecipeJson
        {
            Id = recipe.Id,
            Title = recipe.Title
        };

    }

    public void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}
