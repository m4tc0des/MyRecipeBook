using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Identity;

public interface ILoggerUser
{
    Task<User> Get();

    Guid GetUserId();
}
