using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Infrastructure.DataAcess;

namespace MyRecipeBook.Infrastructure.Repositories;

internal sealed class RecipeRepository: IRecipeWriteOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public RecipeRepository(MyRecipeBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Recipe recipe)
    {
        await _dbContext.AddAsync(recipe);
    }
}
