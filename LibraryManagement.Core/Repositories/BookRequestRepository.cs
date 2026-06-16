using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Repositories;

public class BookRequestRepository
{
    private readonly List<BookRequest> _bookRequests;

    private int _nextId;

    public BookRequestRepository()
    {
        _bookRequests = new List<BookRequest>();
        _nextId = 1;
    }

    public IReadOnlyCollection<BookRequest> GetBookRequests()
    {
        return _bookRequests;
    }

    public BookRequest? FindBookRequestById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Book request id must be positive", nameof(id));

        return _bookRequests.FirstOrDefault(bookRequest => bookRequest.Id.Equals(id));
    }

    public BookRequest AddBookRequest(User creator, Book book, BookRequestType type)
    {
        BookRequest bookRequest = new BookRequest(_nextId, creator, book, type);

        _bookRequests.Add(bookRequest);
        _nextId++;

        return bookRequest;
    }
}
