using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Repositories;

namespace LibraryManagement.Tests.Repositories;

public class BookRepositoryTests
{
    [Fact]
    public void AddBook_NewBook_AddsBook()
    {
        BookRepository bookRepository = new BookRepository();
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);

        Assert.Contains(book, bookRepository.GetBooks());
    }

    [Fact]
    public void AddBook_BookWithSameTitleAndAuthorExists_ThrowsException()
    {
        BookRepository bookRepository = new BookRepository();

        bookRepository.AddBook(new Book("Clean Code", "Robert Martin", 3, 1));

        Assert.Throws<LibraryException>(
            () => bookRepository.AddBook(new Book("Clean Code", "Robert Martin", 5, 1)));
    }

    [Fact]
    public void FindBook_BookExists_ReturnsBook()
    {
        BookRepository bookRepository = new BookRepository();
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        bookRepository.AddBook(book);

        Assert.Same(book, bookRepository.FindBook("Clean Code", "Robert Martin"));
    }

    [Fact]
    public void FindBook_BookDoesNotExist_ReturnsNull()
    {
        BookRepository bookRepository = new BookRepository();

        Assert.Null(bookRepository.FindBook("Clean Code", "Robert Martin"));
    }
}
