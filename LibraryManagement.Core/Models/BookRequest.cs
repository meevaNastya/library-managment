using LibraryManagement.Core.Exceptions;

namespace LibraryManagement.Core.Models;

public class BookRequest
{
    public BookRequest(int id, User creator, Book book, BookRequestType type)
    {
        if (id <= 0)
            throw new ArgumentException("Book request id must be positive", nameof(id));

        if (creator is null)
            throw new ArgumentNullException(nameof(creator));

        if (book is null)
            throw new ArgumentNullException(nameof(book));

        if (type.Equals(BookRequestType.Unknown))
            throw new ArgumentException("Book request type is unknown", nameof(type));

        Id = id;
        Creator = creator;
        Book = book;
        Type = type;
        Status = BookRequestStatus.Created;
    }

    public int Id { get; }

    public User Creator { get; }

    public Book Book { get; }

    public BookRequestType Type { get; }

    public BookRequestStatus Status { get; private set; }

    public void Approve()
    {
        if (!Status.Equals(BookRequestStatus.Created))
            throw new LibraryException("Book request is already reviewed");

        Status = BookRequestStatus.Approved;
    }

    public void Reject()
    {
        if (!Status.Equals(BookRequestStatus.Created))
            throw new LibraryException("Book request is already reviewed");

        Status = BookRequestStatus.Rejected;
    }
}
