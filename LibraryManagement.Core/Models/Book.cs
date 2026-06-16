using LibraryManagement.Core.Exceptions;

namespace LibraryManagement.Core.Models;

public class Book
{
    public Book(string title, string authorName, int copiesLimit, int copiesCount = 0)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Book title is empty", nameof(title));

        if (string.IsNullOrWhiteSpace(authorName))
            throw new ArgumentException("Book author name is empty", nameof(authorName));

        if (copiesLimit <= 0)
            throw new ArgumentException("Book copies limit must be positive", nameof(copiesLimit));

        if (copiesCount < 0)
            throw new ArgumentException("Book copies count is negative", nameof(copiesCount));

        if (copiesCount > copiesLimit)
            throw new ArgumentException("Book copies count exceeds copies limit", nameof(copiesCount));

        Title = title;
        AuthorName = authorName;
        CopiesCount = copiesCount;
        CopiesLimit = copiesLimit;
    }

    public string Title { get; }

    public string AuthorName { get; }

    public int CopiesCount { get; private set; }

    public int CopiesLimit { get; }

    public bool CanAddCopy()
    {
        return CopiesCount < CopiesLimit;
    }

    public bool CanTakeCopy()
    {
        return CopiesCount > 0;
    }

    public void AddCopy()
    {
        if (!CanAddCopy())
            throw new LibraryException("Book copies limit exceeded");

        CopiesCount++;
    }

    public void TakeCopy()
    {
        if (!CanTakeCopy())
            throw new LibraryException("Book has no available copies");

        CopiesCount--;
    }

    public void ReturnCopy()
    {
        if (!CanAddCopy())
            throw new LibraryException("Book copies limit exceeded");

        CopiesCount++;
    }
}
