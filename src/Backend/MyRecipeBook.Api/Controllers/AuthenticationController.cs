using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.RequestCode;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.VerificationCode;

namespace MyRecipeBook.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginWithEmailAndPasswordUseCase useCase, [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPost("password-recovery")]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PasswordRecovery([FromServices] IRequestPasswordRecoveryCodeUseCase useCase, [FromBody] RequestPasswordRecoveryJson request)
    {
        await useCase.Execute(request);

        return Accepted();
    }
}
