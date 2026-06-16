using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Tests.Models;

public class BookTests
{
    [Fact]
    public void Constructor_ValidData_CreatesBook()
    {
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        Assert.Equal("Clean Code", book.Title);
        Assert.Equal("Robert Martin", book.AuthorName);
        Assert.Equal(1, book.CopiesCount);
        Assert.Equal(3, book.CopiesLimit);
    }

    [Fact]
    public void Constructor_EmptyTitle_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Book(" ", "Robert Martin", 3));
    }

    [Fact]
    public void Constructor_CopiesCountExceedsLimit_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Book("Clean Code", "Robert Martin", 3, 4));
    }

    [Fact]
    public void AddCopy_CopiesLimitIsNotReached_IncreasesCopiesCount()
    {
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        book.AddCopy();

        Assert.Equal(2, book.CopiesCount);
    }

    [Fact]
    public void AddCopy_CopiesLimitIsReached_ThrowsException()
    {
        Book book = new Book("Clean Code", "Robert Martin", 1, 1);

        Assert.Throws<LibraryException>(() => book.AddCopy());
    }

    [Fact]
    public void TakeCopy_BookHasCopies_DecreasesCopiesCount()
    {
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        book.TakeCopy();

        Assert.Equal(0, book.CopiesCount);
    }

    [Fact]
    public void TakeCopy_BookHasNoCopies_ThrowsException()
    {
        Book book = new Book("Clean Code", "Robert Martin", 3);

        Assert.Throws<LibraryException>(() => book.TakeCopy());
    }

    [Fact]
    public void ReturnCopy_CopiesLimitIsNotReached_IncreasesCopiesCount()
    {
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        book.ReturnCopy();

        Assert.Equal(2, book.CopiesCount);
    }
}
