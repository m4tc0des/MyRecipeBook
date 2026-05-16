using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RequestRegisterUserJson request, [FromServices] IRegisterUserUseCase useCase)
        {
            await useCase.Execute(request);
            
            return Created();
        }
    }
}
