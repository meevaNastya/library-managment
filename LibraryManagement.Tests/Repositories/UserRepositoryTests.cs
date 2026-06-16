using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Core.Repositories;

namespace LibraryManagement.Tests.Repositories;

public class UserRepositoryTests
{
    [Fact]
    public void AddUser_NewUser_AddsUser()
    {
        UserRepository userRepository = new UserRepository();
        User user = new User("Ivan", [UserRole.Reader]);

        userRepository.AddUser(user);

        Assert.Contains(user, userRepository.GetUsers());
    }

    [Fact]
    public void AddUser_UserWithSameNameExists_ThrowsException()
    {
        UserRepository userRepository = new UserRepository();

        userRepository.AddUser(new User("Ivan", [UserRole.Reader]));

        Assert.Throws<LibraryException>(() => userRepository.AddUser(new User("Ivan", [UserRole.Writer])));
    }

    [Fact]
    public void FindUserByName_UserExists_ReturnsUser()
    {
        UserRepository userRepository = new UserRepository();
        User user = new User("Ivan", [UserRole.Reader]);

        userRepository.AddUser(user);

        Assert.Same(user, userRepository.FindUserByName("Ivan"));
    }

    [Fact]
    public void FindUserByName_UserDoesNotExist_ReturnsNull()
    {
        UserRepository userRepository = new UserRepository();

        Assert.Null(userRepository.FindUserByName("Ivan"));
    }
}
