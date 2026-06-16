using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Tests.Models;

public class BookRequestTests
{
    [Fact]
    public void Constructor_ValidData_CreatesCreatedRequest()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);
        BookRequest bookRequest = new BookRequest(1, user, book, BookRequestType.TakeBook);

        Assert.Equal(1, bookRequest.Id);
        Assert.Same(user, bookRequest.Creator);
        Assert.Same(book, bookRequest.Book);
        Assert.Equal(BookRequestType.TakeBook, bookRequest.Type);
        Assert.Equal(BookRequestStatus.Created, bookRequest.Status);
    }

    [Fact]
    public void Constructor_NotPositiveId_ThrowsException()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        Assert.Throws<ArgumentException>(() => new BookRequest(0, user, book, BookRequestType.TakeBook));
    }

    [Fact]
    public void Approve_CreatedRequest_ChangesStatus()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);
        BookRequest bookRequest = new BookRequest(1, user, book, BookRequestType.TakeBook);

        bookRequest.Approve();

        Assert.Equal(BookRequestStatus.Approved, bookRequest.Status);
    }

    [Fact]
    public void Reject_CreatedRequest_ChangesStatus()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);
        BookRequest bookRequest = new BookRequest(1, user, book, BookRequestType.TakeBook);

        bookRequest.Reject();

        Assert.Equal(BookRequestStatus.Rejected, bookRequest.Status);
    }

    [Fact]
    public void Reject_ApprovedRequest_ThrowsException()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);
        BookRequest bookRequest = new BookRequest(1, user, book, BookRequestType.TakeBook);

        bookRequest.Approve();

        Assert.Throws<LibraryException>(() => bookRequest.Reject());
    }
}
