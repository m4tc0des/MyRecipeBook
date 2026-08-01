using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Mappings;

internal static class MapsterConfiguration
{
    internal static void Configure()
    {
        TypeAdapterConfig<RequestRecipeJson, Recipe>
            .NewConfig()
            .Map(dest => dest.Ingredients, request => request.Ingredients.Select(ingredient => new RecipeIngredient
            {
                Item = ingredient
            }))
            .Map(dest => dest.DishTypes, request => request.DishTypes.Select(dishTypes => new RecipeDishType
            {
                Type = (Domain.Enums.DishType)dishTypes
            }));
    }
}
