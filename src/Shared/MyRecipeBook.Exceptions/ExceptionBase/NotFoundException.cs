using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class NotFoundException : MyRecipeBookException
{
    private readonly string _message;

    public NotFoundException(string message)
    {
        _message = message;
    }

    public override List<string> GetErrorMessages()
    {
        return new List<string> { _message };
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.NotFound;
    }
}
