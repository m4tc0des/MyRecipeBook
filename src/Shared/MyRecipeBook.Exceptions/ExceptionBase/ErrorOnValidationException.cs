using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    private readonly List<string> _errors;
    public ErrorOnValidationException(List<string> errorMessages)
    {
        _errors = errorMessages;
    }

    public override List<string> GetErrorMessages()
    {
        return _errors;
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }
}
