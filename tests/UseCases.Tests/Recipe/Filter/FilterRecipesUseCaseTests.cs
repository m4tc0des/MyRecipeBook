using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Filter;
using MyRecipeBook.Communication.Requests;
using Shouldly;

namespace UseCases.Tests.Recipe.Filter;

public class FilterRecipesUseCaseTests
{
    [Fact]
    public async Task Success_When_RequestIsNull()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var useCase = CreateUseCase(user, [recipe]);
        var result = await useCase.Execute(null);

        result.ShouldNotBeNull();
        result.Recipes.Count.ShouldBe(1);
        result.Recipes.First().ShouldSatisfyAllConditions(
            recipeSummary => recipeSummary.Id.ShouldBe(recipe.Id),
            recipeSummary => recipeSummary.Title.ShouldBe(recipe.Title)
        );
    }

    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var useCase = CreateUseCase(user, [recipe]);
        var request = new RequestFilterRecipeJson();
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Recipes.Count.ShouldBe(1);
        result.Recipes.First().ShouldSatisfyAllConditions(
            recipeSummary => recipeSummary.Id.ShouldBe(recipe.Id),
            recipeSummary => recipeSummary.Title.ShouldBe(recipe.Title)
        );
    }

    private static FilterRecipesUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeReadOnlyRepositoryBuilder().FilterRecipes(user, recipes).Build();

        return new FilterRecipesUseCase(loggedUser, repository);
    }
}
