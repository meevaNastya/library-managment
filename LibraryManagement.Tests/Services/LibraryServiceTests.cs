using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Repositories;
using LibraryManagement.Core.Services;

namespace LibraryManagement.Tests.Services;

public class LibraryServiceTests
{
    [Fact]
    public void CreateTakeBookRequest_ReaderCreatesRequest_CreatesRequest()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateTakeBookRequest(book);

        Assert.Equal(BookRequestType.TakeBook, bookRequest.Type);
        Assert.Equal(BookRequestStatus.Created, bookRequest.Status);
        Assert.Same(reader, bookRequest.Creator);
        Assert.Same(book, bookRequest.Book);
    }

    [Fact]
    public void ApproveBookRequest_LibrarianApprovesTakeRequest_GivesBookToReader()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        User librarian = new User("Petr", [UserRole.Librarian]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateTakeBookRequest(book);
        userSession.Login(librarian);
        libraryService.ApproveBookRequest(bookRequest);

        Assert.Equal(BookRequestStatus.Approved, bookRequest.Status);
        Assert.Contains(book, reader.TakenBooks);
        Assert.Equal(0, book.CopiesCount);
    }

    [Fact]
    public void CreateTakeBookRequest_ReaderAlreadyHasBook_ThrowsException()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        reader.TakeBook(book);
        userSession.Login(reader);

        Assert.Throws<LibraryException>(() => libraryService.CreateTakeBookRequest(book));
    }

    [Fact]
    public void CreateTakeBookRequest_CurrentUserIsNotReader_ThrowsException()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User writer = new User("Ivan", [UserRole.Writer]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        userSession.Login(writer);

        Assert.Throws<LibraryException>(() => libraryService.CreateTakeBookRequest(book));
    }

    [Fact]
    public void ApproveBookRequest_CurrentUserIsNotLibrarian_ThrowsException()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateTakeBookRequest(book);

        Assert.Throws<LibraryException>(() => libraryService.ApproveBookRequest(bookRequest));
    }

    [Fact]
    public void ApproveBookRequest_BookHasNoCopies_ThrowsException()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        User librarian = new User("Petr", [UserRole.Librarian]);
        Book book = new Book("Clean Code", "Robert Martin", 3);

        bookRepository.AddBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateTakeBookRequest(book);
        userSession.Login(librarian);

        Assert.Throws<LibraryException>(() => libraryService.ApproveBookRequest(bookRequest));
        Assert.Equal(BookRequestStatus.Created, bookRequest.Status);
    }

    [Fact]
    public void CreateReturnBookRequest_ReaderHasBook_CreatesRequest()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3);

        bookRepository.AddBook(book);
        reader.TakeBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateReturnBookRequest(book);

        Assert.Equal(BookRequestType.ReturnBook, bookRequest.Type);
        Assert.Equal(BookRequestStatus.Created, bookRequest.Status);
    }

    [Fact]
    public void ApproveBookRequest_LibrarianApprovesReturnRequest_ReturnsBookToLibrary()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        User librarian = new User("Petr", [UserRole.Librarian]);
        Book book = new Book("Clean Code", "Robert Martin", 3);

        bookRepository.AddBook(book);
        reader.TakeBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateReturnBookRequest(book);
        userSession.Login(librarian);
        libraryService.ApproveBookRequest(bookRequest);

        Assert.Equal(BookRequestStatus.Approved, bookRequest.Status);
        Assert.DoesNotContain(book, reader.TakenBooks);
        Assert.Equal(1, book.CopiesCount);
    }

    [Fact]
    public void CreateReturnBookRequest_ReaderDoesNotHaveBook_ThrowsException()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        userSession.Login(reader);

        Assert.Throws<LibraryException>(() => libraryService.CreateReturnBookRequest(book));
    }

    [Fact]
    public void CreateAddBookRequest_WriterCreatesRequest_CreatesRequest()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User writer = new User("Ivan", [UserRole.Writer]);
        Book book = new Book("First Book", "Ivan", 2);

        userSession.Login(writer);
        BookRequest bookRequest = libraryService.CreateAddBookRequest(book);

        Assert.Equal(BookRequestType.AddBook, bookRequest.Type);
        Assert.Equal(BookRequestStatus.Created, bookRequest.Status);
        Assert.Same(writer, bookRequest.Creator);
    }

    [Fact]
    public void ApproveBookRequest_LibrarianApprovesAddRequest_AddsBookCopy()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User writer = new User("Ivan", [UserRole.Writer]);
        User librarian = new User("Petr", [UserRole.Librarian]);
        Book book = new Book("First Book", "Ivan", 2);

        userSession.Login(writer);
        BookRequest bookRequest = libraryService.CreateAddBookRequest(book);
        userSession.Login(librarian);
        libraryService.ApproveBookRequest(bookRequest);

        Assert.Equal(BookRequestStatus.Approved, bookRequest.Status);
        Assert.Contains(book, libraryService.GetBooks());
        Assert.Equal(1, book.CopiesCount);
    }

    [Fact]
    public void ApproveBookRequest_TwoPendingAddRequests_AddsCopiesToStoredBook()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User writer = new User("Ivan", [UserRole.Writer]);
        User librarian = new User("Petr", [UserRole.Librarian]);
        Book firstBook = new Book("First Book", "Ivan", 2);
        Book secondBook = new Book("First Book", "Ivan", 2);

        userSession.Login(writer);
        BookRequest firstBookRequest = libraryService.CreateAddBookRequest(firstBook);
        BookRequest secondBookRequest = libraryService.CreateAddBookRequest(secondBook);
        userSession.Login(librarian);
        libraryService.ApproveBookRequest(firstBookRequest);
        libraryService.ApproveBookRequest(secondBookRequest);

        Assert.Contains(firstBook, libraryService.GetBooks());
        Assert.DoesNotContain(secondBook, libraryService.GetBooks());
        Assert.Equal(2, firstBook.CopiesCount);
        Assert.Equal(0, secondBook.CopiesCount);
    }

    [Fact]
    public void CreateAddBookRequest_WriterAddsAnotherWriterBook_ThrowsException()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User writer = new User("Ivan", [UserRole.Writer]);
        Book book = new Book("First Book", "Petr", 2);

        userSession.Login(writer);

        Assert.Throws<LibraryException>(() => libraryService.CreateAddBookRequest(book));
    }

    [Fact]
    public void ApproveBookRequest_BookCopiesLimitIsReached_ThrowsException()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User writer = new User("Ivan", [UserRole.Writer]);
        User librarian = new User("Petr", [UserRole.Librarian]);
        Book book = new Book("First Book", "Ivan", 1, 1);

        bookRepository.AddBook(book);
        userSession.Login(writer);
        BookRequest bookRequest = libraryService.CreateAddBookRequest(book);
        userSession.Login(librarian);

        Assert.Throws<LibraryException>(() => libraryService.ApproveBookRequest(bookRequest));
        Assert.Equal(BookRequestStatus.Created, bookRequest.Status);
    }

    [Fact]
    public void CreateTakeBookRequest_ReaderIsLibrarian_ApprovesRequestAutomatically()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader, UserRole.Librarian]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateTakeBookRequest(book);

        Assert.Equal(BookRequestStatus.Approved, bookRequest.Status);
        Assert.Contains(book, reader.TakenBooks);
        Assert.Equal(0, book.CopiesCount);
    }

    [Fact]
    public void CreateAddBookRequest_WriterIsLibrarian_ApprovesRequestAutomatically()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User writer = new User("Ivan", [UserRole.Writer, UserRole.Librarian]);
        Book book = new Book("First Book", "Ivan", 2);

        userSession.Login(writer);
        BookRequest bookRequest = libraryService.CreateAddBookRequest(book);

        Assert.Equal(BookRequestStatus.Approved, bookRequest.Status);
        Assert.Contains(book, libraryService.GetBooks());
        Assert.Equal(1, book.CopiesCount);
    }

    [Fact]
    public void RejectBookRequest_LibrarianRejectsRequest_ChangesStatus()
    {
        UserSession userSession = new UserSession();
        BookRepository bookRepository = new BookRepository();
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        LibraryService libraryService = new LibraryService(bookRepository, bookRequestRepository, userSession);
        User reader = new User("Ivan", [UserRole.Reader]);
        User librarian = new User("Petr", [UserRole.Librarian]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);
        userSession.Login(reader);
        BookRequest bookRequest = libraryService.CreateTakeBookRequest(book);
        userSession.Login(librarian);
        libraryService.RejectBookRequest(bookRequest);

        Assert.Equal(BookRequestStatus.Rejected, bookRequest.Status);
        Assert.DoesNotContain(book, reader.TakenBooks);
        Assert.Equal(1, book.CopiesCount);
    }
}
