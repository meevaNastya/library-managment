using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Repositories;

namespace LibraryManagement.Core.Services;

public class LibraryService
{
    private readonly BookRepository _bookRepository;
    private readonly BookRequestRepository _bookRequestRepository;
    private readonly UserSession _userSession;

    public LibraryService(
        BookRepository bookRepository,
        BookRequestRepository bookRequestRepository,
        UserSession userSession)
    {
        if (bookRepository is null)
            throw new ArgumentNullException(nameof(bookRepository));

        if (bookRequestRepository is null)
            throw new ArgumentNullException(nameof(bookRequestRepository));

        if (userSession is null)
            throw new ArgumentNullException(nameof(userSession));

        _bookRepository = bookRepository;
        _bookRequestRepository = bookRequestRepository;
        _userSession = userSession;
    }

    public IReadOnlyCollection<Book> GetBooks()
    {
        return _bookRepository.GetBooks();
    }

    public IReadOnlyCollection<BookRequest> GetBookRequests()
    {
        return _bookRequestRepository.GetBookRequests();
    }

    public BookRequest CreateTakeBookRequest(Book book)
    {
        User reader = _userSession.GetCurrentUser();
        Book storedBook = GetStoredBook(book);

        CheckUserRole(reader, UserRole.Reader);

        if (reader.HasBook(storedBook))
            throw new LibraryException("Reader already has this book");

        BookRequest bookRequest = _bookRequestRepository.AddBookRequest(
            reader,
            storedBook,
            BookRequestType.TakeBook);

        ApproveBookRequestIfCreatorIsLibrarian(bookRequest);

        return bookRequest;
    }

    public BookRequest CreateReturnBookRequest(Book book)
    {
        User reader = _userSession.GetCurrentUser();
        Book storedBook = GetStoredBook(book);

        CheckUserRole(reader, UserRole.Reader);

        if (!reader.HasBook(storedBook))
            throw new LibraryException("Reader does not have this book");

        BookRequest bookRequest = _bookRequestRepository.AddBookRequest(
            reader,
            storedBook,
            BookRequestType.ReturnBook);

        ApproveBookRequestIfCreatorIsLibrarian(bookRequest);

        return bookRequest;
    }

    public BookRequest CreateAddBookRequest(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        User writer = _userSession.GetCurrentUser();

        CheckUserRole(writer, UserRole.Writer);
        CheckWriterOwnsBook(writer, book);

        Book requestBook = _bookRepository.FindBook(book.Title, book.AuthorName) ?? book;
        BookRequest bookRequest = _bookRequestRepository.AddBookRequest(
            writer,
            requestBook,
            BookRequestType.AddBook);

        ApproveBookRequestIfCreatorIsLibrarian(bookRequest);

        return bookRequest;
    }

    public void ApproveBookRequest(BookRequest bookRequest)
    {
        if (bookRequest is null)
            throw new ArgumentNullException(nameof(bookRequest));

        User librarian = _userSession.GetCurrentUser();

        CheckUserRole(librarian, UserRole.Librarian);
        ApproveBookRequestWithoutRoleCheck(bookRequest);
    }

    public void RejectBookRequest(BookRequest bookRequest)
    {
        if (bookRequest is null)
            throw new ArgumentNullException(nameof(bookRequest));

        User librarian = _userSession.GetCurrentUser();

        CheckUserRole(librarian, UserRole.Librarian);
        bookRequest.Reject();
    }

    private void ApproveBookRequestIfCreatorIsLibrarian(BookRequest bookRequest)
    {
        if (!bookRequest.Creator.HasRole(UserRole.Librarian))
            return;

        ApproveBookRequestWithoutRoleCheck(bookRequest);
    }

    private void ApproveBookRequestWithoutRoleCheck(BookRequest bookRequest)
    {
        CheckBookRequestIsCreated(bookRequest);

        ApplyBookRequest(bookRequest);
        bookRequest.Approve();
    }

    private void ApplyBookRequest(BookRequest bookRequest)
    {
        switch (bookRequest.Type)
        {
            case BookRequestType.TakeBook:
                ApplyTakeBookRequest(bookRequest);
                break;
            case BookRequestType.ReturnBook:
                ApplyReturnBookRequest(bookRequest);
                break;
            case BookRequestType.AddBook:
                ApplyAddBookRequest(bookRequest);
                break;
            default:
                throw new LibraryException("Book request type is unknown");
        }
    }

    private void ApplyTakeBookRequest(BookRequest bookRequest)
    {
        User reader = bookRequest.Creator;
        Book book = bookRequest.Book;

        CheckUserRole(reader, UserRole.Reader);

        if (reader.HasBook(book))
            throw new LibraryException("Reader already has this book");

        book.TakeCopy();
        reader.TakeBook(book);
    }

    private void ApplyReturnBookRequest(BookRequest bookRequest)
    {
        User reader = bookRequest.Creator;
        Book book = bookRequest.Book;

        CheckUserRole(reader, UserRole.Reader);

        if (!reader.HasBook(book))
            throw new LibraryException("Reader does not have this book");

        reader.ReturnBook(book);
        book.ReturnCopy();
    }

    private void ApplyAddBookRequest(BookRequest bookRequest)
    {
        User writer = bookRequest.Creator;
        Book book = bookRequest.Book;

        CheckUserRole(writer, UserRole.Writer);
        CheckWriterOwnsBook(writer, book);

        Book? storedBook = _bookRepository.FindBook(book.Title, book.AuthorName);

        if (storedBook is null)
        {
            _bookRepository.AddBook(book);
            storedBook = book;
        }

        storedBook.AddCopy();
    }

    private Book GetStoredBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        Book? storedBook = _bookRepository.FindBook(book.Title, book.AuthorName);

        if (storedBook is null)
            throw new LibraryException("Book was not found");

        return storedBook;
    }

    private void CheckUserRole(User user, UserRole role)
    {
        if (!user.HasRole(role))
            throw new LibraryException("User does not have required role");
    }

    private void CheckWriterOwnsBook(User writer, Book book)
    {
        if (!book.AuthorName.Equals(writer.Name, StringComparison.Ordinal))
            throw new LibraryException("Writer can add only own books");
    }

    private void CheckBookRequestIsCreated(BookRequest bookRequest)
    {
        if (!bookRequest.Status.Equals(BookRequestStatus.Created))
            throw new LibraryException("Book request is already reviewed");
    }
}
