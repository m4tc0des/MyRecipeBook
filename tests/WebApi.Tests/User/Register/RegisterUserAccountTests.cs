using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace WebApi.Tests.User.Register;

public class RegisterUserAccountTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    public RegisterUserAccountTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        await _httpClient.PostAsJsonAsync("/users", request);
    }

    [Fact]
    public async Task Validate_ShouldBeErrorResponse_WhenNameIsEmpty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Name = string.Empty;

        await _httpClient.PostAsJsonAsync("/users", request);
    }
}
