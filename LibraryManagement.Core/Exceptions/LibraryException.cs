namespace LibraryManagement.Core.Exceptions;

public class LibraryException : Exception
{
    public LibraryException(string message)
        : base(message)
    {
    }
}
