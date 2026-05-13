namespace MyRecipeBook.Exceptions.ExceptionBase;

public class ErrorOnValidationException: MyRecipeBookException
{
    private readonly List<string> _errorMessages;
    public ErrorOnValidationException(List<string> errorMessages)
    {
        _errorMessages = errorMessages;
    }

    public List<string> GetErrorMessages()
    {
        return _errorMessages;
    }
}
