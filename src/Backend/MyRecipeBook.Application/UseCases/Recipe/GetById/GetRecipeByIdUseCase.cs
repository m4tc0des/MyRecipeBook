using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    public Task<ResponseRecipeJson> Execute(Guid recipeId)
    {
        throw new NotImplementedException();
    }
}
