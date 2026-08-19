using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.UpdateById;

public class UpdateRecipeTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes";
    private readonly UserIdentityManager _userOne;

    public UpdateRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _userOne = factory.User_One;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _userOne.GetRecipe();
        var request = RequestRecipeJsonBuilder.Build();
        var response = await Put($"{REQUEST_URI}/{recipe.Id}", request, accessToken: _userOne.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var recipeUpdated = await DbContext.Recipes.AnyAsync(entity =>
        entity.Id == recipe.Id &&
        entity.Active &&
        entity.Title.Equals(request.Title) &&
        entity.UserId == _userOne.GetId());

        recipeUpdated.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_When_RecipeNotFound(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();
        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: _userOne.GetAccessToken(), culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_RECIPE_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorList =>
        {
            errorList.Count().ShouldBe(1);
            errorList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_When_TitleIsEmpty(string culture)
    {
        var recipe = _userOne.GetRecipe();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;
        var response = await Put($"{REQUEST_URI}/{recipe.Id}", request, accessToken: _userOne.GetAccessToken(), culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_TITLE_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorList =>
        {
            errorList.Count().ShouldBe(1);
            errorList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
