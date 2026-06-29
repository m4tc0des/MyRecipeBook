using Bogus;
using CommonTestUtilities.Security;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (MyRecipeBook.Domain.Entities.User user, string password) Build()
    {
        var (password, passwordHashed) = GenerateRandomPassword();

        var user = new Faker<MyRecipeBook.Domain.Entities.User>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, _ => passwordHashed);

        return (user, password);
    }

    private static (string password, string stringPasswordHashed) GenerateRandomPassword()
    {
        var passwordEncripter = new IPasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();

        return (password, passwordEncripter.HashPassword(password));
    }
}
