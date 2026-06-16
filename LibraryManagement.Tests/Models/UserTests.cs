using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Tests.Models;

public class UserTests
{
    [Fact]
    public void Constructor_ValidData_CreatesUser()
    {
        User user = new User("Ivan", [UserRole.Reader, UserRole.Librarian]);

        Assert.Equal("Ivan", user.Name);
        Assert.Contains(UserRole.Reader, user.Roles);
        Assert.Contains(UserRole.Librarian, user.Roles);
    }

    [Fact]
    public void Constructor_EmptyName_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new User(" ", [UserRole.Reader]));
    }

    [Fact]
    public void Constructor_EmptyRoles_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new User("Ivan", []));
    }

    [Fact]
    public void Constructor_DuplicateRoles_SavesRoleOnce()
    {
        User user = new User("Ivan", [UserRole.Reader, UserRole.Reader]);

        Assert.Single(user.Roles);
    }

    [Fact]
    public void TakeBook_UserDoesNotHaveBook_AddsBook()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        user.TakeBook(book);

        Assert.Contains(book, user.TakenBooks);
    }

    [Fact]
    public void TakeBook_UserAlreadyHasBook_ThrowsException()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        user.TakeBook(book);

        Assert.Throws<LibraryException>(() => user.TakeBook(book));
    }

    [Fact]
    public void ReturnBook_UserHasBook_RemovesBook()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        user.TakeBook(book);
        user.ReturnBook(book);

        Assert.DoesNotContain(book, user.TakenBooks);
    }

    [Fact]
    public void ReturnBook_UserDoesNotHaveBook_ThrowsException()
    {
        User user = new User("Ivan", [UserRole.Reader]);
        Book book = new Book("Clean Code", "Robert Martin", 3, 1);

        Assert.Throws<LibraryException>(() => user.ReturnBook(book));
    }
}
