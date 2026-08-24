using Shouldly;
using System.Net;

namespace WebApi.Tests.Recipe.Recent;

public class GetRecentRecipeInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes/recent";

    private readonly string _tokenUserNotExistDatabase;

    public GetRecentRecipeInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_When_AccessTokenIsInvalid()
    {
        var response = await Get(REQUEST_URI, accessToken: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_When_AccessTokenIsMissing()
    {
        var response = await Get(REQUEST_URI, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_When_UserFromAccessTokenDoesNotExist()
    {
        var response = await Get(REQUEST_URI, accessToken: _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
