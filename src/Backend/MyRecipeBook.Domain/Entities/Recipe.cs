using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<RecipeInstruction> Instructions{ get; set; } = new List<RecipeInstruction>();
    public ICollection<RecipeDishType> DishTypes{ get; set; } = new List<RecipeDishType>();
    public CookTime CookTime { get; set; }
    public Guid UserId { get; set; }
}
