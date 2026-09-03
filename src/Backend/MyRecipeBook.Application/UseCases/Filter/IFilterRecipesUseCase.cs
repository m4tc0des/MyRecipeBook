using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Filter;

public interface IFilterRecipesUseCase
{
    Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson? request);
}
