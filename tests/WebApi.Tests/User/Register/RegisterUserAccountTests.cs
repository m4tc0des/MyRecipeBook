using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.User.Register;

public class RegisterUserAccountTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/users";

    public RegisterUserAccountTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);

        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty();

        var userExists = await DbContext.Users.AnyAsync(user => user.Active && user.Name.Equals(request.Name) && user.Email.Equals(request.Email));

        userExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeErrorResponse_When_NameIsEmpty(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Name = string.Empty;

        var response = await Post(REQUEST_URI, request, culture: culture);

        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var respondeData = await JsonDocument.ParseAsync(responseBody);

        var errors = respondeData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });

        var userExists = await DbContext.Users.AnyAsync(user => user.Active && user.Name.Equals(request.Name) && user.Email.Equals(request.Email));

        userExists.ShouldBeFalse();
    }
}
