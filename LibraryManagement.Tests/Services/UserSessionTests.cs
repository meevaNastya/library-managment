using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Services;

namespace LibraryManagement.Tests.Services;

public class UserSessionTests
{
    [Fact]
    public void GetCurrentUser_UserIsNotLoggedIn_ThrowsException()
    {
        UserSession userSession = new UserSession();

        Assert.Throws<LibraryException>(() => userSession.GetCurrentUser());
    }

    [Fact]
    public void Login_ValidUser_SetsCurrentUser()
    {
        UserSession userSession = new UserSession();
        User user = new User("Ivan", [UserRole.Reader]);

        userSession.Login(user);

        Assert.True(userSession.HasCurrentUser);
        Assert.Same(user, userSession.GetCurrentUser());
    }

    [Fact]
    public void Logout_UserIsLoggedIn_ClearsCurrentUser()
    {
        UserSession userSession = new UserSession();
        User user = new User("Ivan", [UserRole.Reader]);

        userSession.Login(user);
        userSession.Logout();

        Assert.False(userSession.HasCurrentUser);
    }
}
