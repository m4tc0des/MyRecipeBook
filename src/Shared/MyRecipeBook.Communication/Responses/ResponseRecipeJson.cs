using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Responses;

public class ResponseRecipeJson
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = new List<string>();

    public IList<ResponseInstructionJson> Instructions { get; set; } = new List<ResponseInstructionJson>();

    public IList<DishTypes> DishTypes { get; set; } = new List<DishTypes>();

    public CookTime CookTime { get; set; }
}
