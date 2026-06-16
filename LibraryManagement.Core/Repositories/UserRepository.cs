using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Repositories;

public class UserRepository
{
    private readonly List<User> _users;

    public UserRepository()
    {
        _users = new List<User>();
    }

    public IReadOnlyCollection<User> GetUsers()
    {
        return _users;
    }

    public User? FindUserByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("User name is empty", nameof(name));

        return _users.FirstOrDefault(user => user.Name.Equals(name, StringComparison.Ordinal));
    }

    public void AddUser(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        if (FindUserByName(user.Name) is not null)
            throw new LibraryException("User with this name already exists");

        _users.Add(user);
    }
}
