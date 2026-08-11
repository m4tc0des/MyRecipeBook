using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.Register;

public class RegisterRecipeTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "recipes";

    private readonly UserIdentityManager _userOne;

    public RegisterRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _userOne = factory.User_One;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, accessToken: _userOne.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);

        var recipeId = responseData.RootElement.GetProperty("id").GetGuid();

        var recipeExists = await DbContext.Recipes.AnyAsync(recipe =>
        recipe.Id == recipeId &&
        recipe.Active &&
        recipe.Title.Equals(request.Title) &&
        recipe.UserId == _userOne.GetId());

        recipeExists.ShouldBeTrue();
    }
}


