using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class InvalidLoginException : MyRecipeBookException
{
    public override List<string> GetErrorMessages()
    {
        return [ResourceMessagesException.VALIDATION_LOGIN_INVALID];
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.Unauthorized;
    }
}
