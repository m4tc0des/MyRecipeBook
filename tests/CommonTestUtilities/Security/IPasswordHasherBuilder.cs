using Moq;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;

namespace CommonTestUtilities.Security;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock
        ;
    public IPasswordHasherBuilder()
    {
        _mock = new Mock<IPasswordHasher>();

        _mock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed_password");
    }

    public void ExistActiveUserWithEmail(string password)
    {
        _mock.Setup(x => x.VerifyPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build()
    {
        return _mock.Object;
    }
}
