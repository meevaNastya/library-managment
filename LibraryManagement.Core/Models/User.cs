using LibraryManagement.Core.Exceptions;

namespace LibraryManagement.Core.Models;

public class User
{
    private readonly List<UserRole> _roles;
    private readonly List<Book> _takenBooks;

    public User(string name, IEnumerable<UserRole> roles)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("User name is empty", nameof(name));

        if (roles is null)
            throw new ArgumentNullException(nameof(roles));

        _roles = new List<UserRole>();
        _takenBooks = new List<Book>();

        foreach (UserRole role in roles)
        {
            if (role.Equals(UserRole.Unknown))
                throw new ArgumentException("User role is unknown", nameof(roles));

            if (!_roles.Contains(role))
                _roles.Add(role);
        }

        if (_roles.Count.Equals(0))
            throw new ArgumentException("User roles are empty", nameof(roles));

        Name = name;
    }

    public string Name { get; }

    public IReadOnlyCollection<UserRole> Roles => _roles;

    public IReadOnlyCollection<Book> TakenBooks => _takenBooks;

    public bool HasRole(UserRole role)
    {
        if (role.Equals(UserRole.Unknown))
            throw new ArgumentException("User role is unknown", nameof(role));

        return _roles.Contains(role);
    }

    public bool HasBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        return _takenBooks.Contains(book);
    }

    public void TakeBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        if (HasBook(book))
            throw new LibraryException("User already has this book");

        _takenBooks.Add(book);
    }

    public void ReturnBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        if (!HasBook(book))
            throw new LibraryException("User does not have this book");

        _takenBooks.Remove(book);
    }
}
