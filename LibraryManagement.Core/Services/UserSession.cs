using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Services;

public class UserSession
{
    private User? _currentUser;

    public bool HasCurrentUser => _currentUser is not null;

    public User GetCurrentUser()
    {
        if (_currentUser is null)
            throw new LibraryException("Current user is not selected");

        return _currentUser;
    }

    public void Login(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        _currentUser = user;
    }

    public void Logout()
    {
        _currentUser = null;
    }
}
