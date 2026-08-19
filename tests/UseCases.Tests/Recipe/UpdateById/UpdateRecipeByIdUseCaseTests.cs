using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.UpdateById;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.Recipe.UpdateById;

public class UpdateRecipeByIdUseCaseTests
{
    static UpdateRecipeByIdUseCaseTests()
    {
        MapsterConfiguration.Configure();
    }

    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(user, recipe);

        await useCase.Execute(recipe.Id, request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThowException_When_RecipeNotFound()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(user, recipe);

        var exception = await useCase.Execute(Guid.CreateVersion7(), request).ShouldThrowAsync<NotFoundException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessage =>
        {
            errorMessage.Count.ShouldBe(1);
            errorMessage.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        });

        await useCase.Execute(recipe.Id, request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThowException_When_TitleIsEmpty()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user, recipe);

        var exception = await useCase.Execute(recipe.Id, request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessage =>
        {
            errorMessage.Count.ShouldBe(1);
            errorMessage.ShouldContain(ResourceMessagesException.VALIDATION_TITLE_REQUIRED);
        });
    }

    private static UpdateRecipeByIdUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeUpdateOnlyRepositoryBuilder().GetById(recipe).Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new UpdateRecipeByIdUseCase(loggedUser, repository, unitOfWork);
    }
}
