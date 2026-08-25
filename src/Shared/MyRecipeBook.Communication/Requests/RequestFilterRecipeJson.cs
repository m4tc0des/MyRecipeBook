using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Requests;

public class RequestFilterRecipeJson
{
    public string? SearchItem { get; set; }
    public CookTime? CookTime { get; set; }
    public IList<DishTypes> DishTypes { get; set; } = [];
}
