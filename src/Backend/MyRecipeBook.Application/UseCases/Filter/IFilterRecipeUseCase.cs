using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Filter;

public interface IFilterRecipeUseCase
{
    Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson? request);
}
