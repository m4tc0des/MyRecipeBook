using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.GetById;

public class GetRecipeByIdTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes";

    private readonly UserIdentityManager _userOne;

    public GetRecipeByIdTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _userOne = factory.User_One;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _userOne.GetRecipe();

        var response = await Get($"{REQUEST_URI}/{recipe.Id}", accessToken: _userOne.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetGuid().ShouldBe(recipe.Id);

        responseData.RootElement.GetProperty("title").GetString().ShouldBe(recipe.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse(string culture)
    {
        var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _userOne.GetAccessToken(), culture: culture);

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
}
