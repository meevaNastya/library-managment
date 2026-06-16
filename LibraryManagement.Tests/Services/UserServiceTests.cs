using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Repositories;
using LibraryManagement.Core.Services;

namespace LibraryManagement.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public void RegisterUser_ValidData_AddsUser()
    {
        UserRepository userRepository = new UserRepository();
        UserSession userSession = new UserSession();
        UserService userService = new UserService(userRepository, userSession);

        User user = userService.RegisterUser("Ivan", [UserRole.Reader]);

        Assert.Contains(user, userService.GetUsers());
    }

    [Fact]
    public void LoginUser_UserExists_SetsCurrentUser()
    {
        UserRepository userRepository = new UserRepository();
        UserSession userSession = new UserSession();
        UserService userService = new UserService(userRepository, userSession);
        User user = userService.RegisterUser("Ivan", [UserRole.Reader]);

        User loggedUser = userService.LoginUser("Ivan");

        Assert.Same(user, loggedUser);
        Assert.Same(user, userService.GetCurrentUser());
    }

    [Fact]
    public void LoginUser_UserDoesNotExist_ThrowsException()
    {
        UserRepository userRepository = new UserRepository();
        UserSession userSession = new UserSession();
        UserService userService = new UserService(userRepository, userSession);

        Assert.Throws<LibraryException>(() => userService.LoginUser("Ivan"));
    }

    [Fact]
    public void LogoutUser_UserIsLoggedIn_ClearsCurrentUser()
    {
        UserRepository userRepository = new UserRepository();
        UserSession userSession = new UserSession();
        UserService userService = new UserService(userRepository, userSession);

        userService.RegisterUser("Ivan", [UserRole.Reader]);
        userService.LoginUser("Ivan");
        userService.LogoutUser();

        Assert.Throws<LibraryException>(() => userService.GetCurrentUser());
    }
}
