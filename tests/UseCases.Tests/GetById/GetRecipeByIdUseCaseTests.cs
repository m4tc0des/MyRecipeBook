using MyRecipeBook.Application.Mappings;

namespace UseCases.Tests.GetById;

public  class GetRecipeByIdUseCaseTests
{
    public GetRecipeByIdUseCaseTests()
    {
        MapsterConfiguration.Configure();
    }

    [Fact]
    public async Task Success()
    {
        
    }

    [Fact]
    public async Task Validate_ShouldThrowException_When_RecipeNotFound()
    {
        
    }
}
