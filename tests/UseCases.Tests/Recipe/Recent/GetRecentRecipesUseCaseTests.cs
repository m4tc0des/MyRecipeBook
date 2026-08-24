using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Recipe.Recent;
using Shouldly;

namespace UseCases.Tests.Recipe.Recent;

public class GetRecentRecipesUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, [recipe]);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Recipes.Count.ShouldBe(1);
        result.Recipes.First().ShouldSatisfyAllConditions(
            recipeSummary => recipeSummary.Id.ShouldBe(recipe.Id),
            recipeSummary => recipeSummary.Title.ShouldBe(recipe.Title)
        );
    }

    [Fact]
    public async Task Success_When_ThereAreNoRecipes()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user, []);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();

        result.Recipes.ShouldBeEmpty();
    }

    private static GetRecentRecipesUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);

        var repository = new IRecipeReadOnlyRepositoryBuilder().GetRecentRecipe(user, recipes).Build();

        return new GetRecentRecipesUseCase(loggedUser, repository);
    }
}
