using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UseCases.Tests")]
namespace MyRecipeBook.Application.Mappings;

internal static class MapsterConfiguration
{
    internal static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserJson, User>
            .NewConfig()
            .Ignore(dest => dest.Password);

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

        TypeAdapterConfig<Recipe, ResponseRecipeJson>
            .NewConfig()
            .Map(dest => dest.Ingredients, entity => entity.Ingredients.Select(ingredient => ingredient.Item))
            .Map(dest => dest.DishTypes, entity => entity.DishTypes.Select(dishTypes => dishTypes.Type));
    }
}
