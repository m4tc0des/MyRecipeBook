using CommonTestUtilities.Requests;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Login;

public class LoginWithEmailAndPasswordTests: IClassFixture<MyRecipeBookApplicationFactory>
{
    private const string REQUEST_URI = "/authentication";

    private readonly HttpClient _httpClient;

    private readonly UserIdentityManager _userOne;

    public LoginWithEmailAndPasswordTests(MyRecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        _userOne = factory.User_One;
    }

    [Fact]
    public async Task Sucess()
    {
        var request = new RequestLoginJson
        {
            Email = _userOne.GetEmail(),
            Password = _userOne.GetPassword(),
        };

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_userOne.GetName());

        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowException_When_UserDontExist(string culture)
    {
        var request = RequestLoginJsonBuilder.Build(); 

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_LOGIN_INVALID", new CultureInfo(culture));

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var respondeData = await JsonDocument.ParseAsync(responseBody);

        var errors = respondeData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
