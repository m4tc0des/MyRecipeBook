using MyRecipeBook.Communication.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Tests.User.Register;

public class GetUserProfileInvalidTokenTests: BaseIntegrationTest
{
    private readonly string REQUEST_URI = "users";

    private readonly string _tokenUserNotExistDatabase;

    public GetUserProfileInvalidTokenTests(MyRecipeBookApplicationFactory factory): base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = new RequestChangePasswordJson();

        var response = await Get(REQUEST_URI, accessToken: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = new RequestChangePasswordJson();

        var response = await Get(REQUEST_URI, accessToken: _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
