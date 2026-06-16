using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Repositories;

namespace LibraryManagement.Core.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly UserSession _userSession;

    public UserService(UserRepository userRepository, UserSession userSession)
    {
        if (userRepository is null)
            throw new ArgumentNullException(nameof(userRepository));

        if (userSession is null)
            throw new ArgumentNullException(nameof(userSession));

        _userRepository = userRepository;
        _userSession = userSession;
    }

    public IReadOnlyCollection<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }

    public User GetCurrentUser()
    {
        return _userSession.GetCurrentUser();
    }

    public User RegisterUser(string name, IEnumerable<UserRole> roles)
    {
        User user = new User(name, roles);

        _userRepository.AddUser(user);

        return user;
    }

    public User LoginUser(string name)
    {
        User? user = _userRepository.FindUserByName(name);

        if (user is null)
            throw new LibraryException("User with this name was not found");

        _userSession.Login(user);

        return user;
    }

    public void LogoutUser()
    {
        _userSession.Logout();
    }
}
