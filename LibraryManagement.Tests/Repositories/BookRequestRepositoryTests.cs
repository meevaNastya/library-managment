using LibraryManagement.Core.Models;
using LibraryManagement.Core.Repositories;

namespace LibraryManagement.Tests.Repositories;

public class BookRequestRepositoryTests
{
    [Fact]
    public void AddBookRequest_ValidData_AddsBookRequest()
    {
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        BookRequest bookRequest = bookRequestRepository.AddBookRequest(
            user,
            book,
            BookRequestType.TakeBook);

        Assert.Contains(bookRequest, bookRequestRepository.GetBookRequests());
    }

    [Fact]
    public void AddBookRequest_TwoRequests_CreatesDifferentIds()
    {
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        BookRequest firstBookRequest = bookRequestRepository.AddBookRequest(
            user,
            book,
            BookRequestType.TakeBook);
        BookRequest secondBookRequest = bookRequestRepository.AddBookRequest(
            user,
            book,
            BookRequestType.TakeBook);

        Assert.NotEqual(firstBookRequest.Id, secondBookRequest.Id);
    }

    [Fact]
    public void FindBookRequestById_BookRequestExists_ReturnsBookRequest()
    {
        BookRequestRepository bookRequestRepository = new BookRequestRepository();
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);
        BookRequest bookRequest = bookRequestRepository.AddBookRequest(
            user,
            book,
            BookRequestType.TakeBook);

        Assert.Same(bookRequest, bookRequestRepository.FindBookRequestById(bookRequest.Id));
    }
}
