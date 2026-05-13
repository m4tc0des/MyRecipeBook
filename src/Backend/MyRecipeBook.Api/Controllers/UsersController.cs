using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register([FromBody]RequestRegisterUserJson request)
        {
            var useCase = new RegisterUserUseCase();

            useCase.Execute(request);
            
            return Created();
        }
    }
}
