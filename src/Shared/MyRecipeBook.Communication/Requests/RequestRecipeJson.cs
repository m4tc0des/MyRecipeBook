using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Requests;

public class RequestRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public CookTime CookTime { get; set; }
    public IList<string> Ingredients { get; set; } = new List<string>();
    public IList<RequestRecipeInstructionJson> Instructions { get; set; } = new List<RequestRecipeInstructionJson>();
    public IList<DishTypes> DishTypes { get; set; } = new List<DishTypes>();

}
