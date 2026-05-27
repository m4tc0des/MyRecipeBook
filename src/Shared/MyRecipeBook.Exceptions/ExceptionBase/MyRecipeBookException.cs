using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase
{
    public abstract class MyRecipeBookException : Exception
    {
        public abstract HttpStatusCode GetStatusCode();

        public abstract List<string> GetErrorMessages();
    }
}
