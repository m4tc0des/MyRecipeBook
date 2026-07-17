using Bogus;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.Requests;

public class RequestRecipeJsonBuilder
{
    public static RequestRecipeJson Build()
    {
        var instructionOrder = 1;

        return new Faker<RequestRecipeJson>()
            .RuleFor(request => request.Title, f => f.Lorem.Word())
            .RuleFor(request => request.CookTime, f => f.PickRandom<MyRecipeBook.Communication.Enums.CookTime>())
            .RuleFor(request => request.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(request => request.DishTypes, f => Enumerable.Range(1, 2).Select(_ => f.PickRandom<MyRecipeBook.Communication.Enums.DishTypes>()).Distinct().ToList())
            .RuleFor(request => request.Instructions, f => f.Make(3, () => new RequestRecipeInstructionJson
            {
                Order = instructionOrder++,
                Description = f.Lorem.Sentence(),
            }));
    }
}