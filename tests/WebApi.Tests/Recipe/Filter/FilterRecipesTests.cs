using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.Filter;

public class FilterRecipesTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes/filter";

    private readonly UserIdentityManager _userOne;

    public FilterRecipesTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _userOne = factory.User_One;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _userOne.GetRecipe();

        var request = new RequestFilterRecipeJson
        {
            SearchItem = recipe.Title,
            CookTime = (CookTime)recipe.CookTime,
            DishTypes = recipe.DishTypes.Select(dishType => (DishTypes)dishType.Type).ToList()
        };

        var response = await Post(REQUEST_URI, request, accessToken: _userOne.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").ValueKind.ShouldBe(JsonValueKind.Array);

        var recipes = responseData.RootElement.GetProperty("recipes").EnumerateArray();

        recipes.ShouldNotBeEmpty();

        recipes.ShouldContain(r =>
            r.GetProperty("id").GetGuid() == recipe.Id &&
            r.GetProperty("title").GetString() == recipe.Title);
    }
}
