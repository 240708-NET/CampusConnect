using Moq;                                  //For Setup, Verify, ReturnsAsync
using Microsoft.AspNetCore.Mvc;             //For ActionResult, CreatedAtActionResult, etc.
using BlogAPI.Controllers;                  //UserController.cs is in here
using BlogAPI.Models;                       //User.cs is in here
using BlogAPI.Repositories;                 //IUserRepository.cs is in here

namespace BlogAPITests;

public class UserControllerTests
{
    //Using a mock repository to isolate unit tests from actual database
    private readonly Mock<IUserRepository> _mockRepository;
    //Instance of class we are testing
    private readonly UserController _controller;

    public UserControllerTests()
    {
        //Initializing mock repository
        _mockRepository = new Mock<IUserRepository>();
        //Passing mock repository object to UserController constructor
        _controller = new UserController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetUsers_ReturnsAllUsers()
    {
        //ARRANGE
        //Creating a list of users, and initializing it with 2 users
        List<User> users = new List<User>
        {
            new User { ID = 1, Username = "User1", Password = "Password1" },
            new User { ID = 2, Username = "User2", Password = "Password2" }
        };

        //Configuring behavior of _mockRepository, returning user list asynchronously 
        _mockRepository.Setup(repo => repo.Get()).ReturnsAsync(users);

        //ACT
        //Calling GetUsers() method
        var result = await _controller.GetUsers();

        //ASSERT
        //Verifying result is of type ActionResult<List<User>>
        var actionResult = Assert.IsType<ActionResult<List<User>>>(result);
        //Verifying that the value of actionResult is List<User>.
        var returnValue = Assert.IsType<List<User>>(actionResult.Value);
        //Verifying that returnValue has a count of 2
        Assert.Equal(2, returnValue.Count);
    }

    [Theory]
    [InlineData(1, "User1", "Password1")]
    [InlineData(2, "User2", "Password2")]
    public async Task GetUser_ReturnsUserById(int id, string username, string password)
    {
        //ARRANGE
        //Creating a user object with parameters
        User user = new User { ID = id, Username = username, Password = password };
        //Configuring behavior of _mockRepository, return user asynchronously
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(user);

        //ACT
        //Calling GetUser(int id) method
        var result = await _controller.GetUser(id);

        //ASSERT
        //Verifying result is of type ActionResult<User>
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        //Verifying that the value of actionResult is User
        var returnValue = Assert.IsType<User>(actionResult.Value);
        //Verifying that the returnValue properties match the values from the User object
        Assert.Equal(user.ID, returnValue.ID);
        Assert.Equal(user.Username, returnValue.Username);
    }

    [Theory]
    [InlineData(1, "NewUser1", "Password1")]
    [InlineData(2, "NewUser2", "Password2")]
    public async Task PostUser_CreatesNewUser(int id, string username, string password)
    {
        //ARRANGE
        //Creating a user object with parameters
        User user = new User { ID = id, Username = username, Password = password };
        //Configuring behavior of _mockRepository. Task.CompletedTask indicates successful operation
        _mockRepository.Setup(repo => repo.Insert(user)).Returns(Task.CompletedTask);

        //ACT
        //Calling PostUser(User user) method
        var result = await _controller.PostUser(user);

        //ASSERT
        //Verifying result is of type ActionResult<User>
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        //Verifying that Result property of actionResult is of type CreatedAtActionResult
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //Verifying that Value property of createdAtActionResult is of type User
        var returnValue = Assert.IsType<User>(createdAtActionResult.Value);
        //Verifying that Username of returnValue matches Username of User object
        Assert.Equal(user.Username, returnValue.Username);
    }

    [Theory]
    [InlineData(1, "UpdatedUser1", "UpdatedPassword1")]
    [InlineData(2, "UpdatedUser2", "UpdatedPassword2")]
    public async Task PutUser_UpdatesExistingUser(int id, string username, string password)
    {
        //ARRANGE
        //Creating objects for an "existing" user, and a user that will represent the changes to be made
        User existingUser = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" };
        User updatedUser = new User { ID = id, Username = username, Password = password };

        //Configuring behavior of _mockRepository. Return existingUser asynchronously and true indicating successful update
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(existingUser);
        _mockRepository.Setup(repo => repo.Update(updatedUser)).ReturnsAsync(true);

        //ACT
        //Calling PutUser(int id, User user) method
        var result = await _controller.PutUser(id, updatedUser);

        //ASSERT
        //Verifying that the result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the Update method was called exactly once
        _mockRepository.Verify(repo => repo.Update(updatedUser), Times.Once);
    }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteUser_DeletesExistingUser(int id)
        {
            //ARRANGE
            //Configuring behavior of _mockRepository to return true indicating successful delete
            _mockRepository.Setup(repo => repo.DeleteById(id)).ReturnsAsync(true);

            //ACT
            //Calling DeleteUser(int id) method
            var result = await _controller.DeleteUser(id);

            //ASSERT
            //Verifying that result is of type NoContentResult
            Assert.IsType<NoContentResult>(result);
            //Verifying that the DeleteById method was called exactly once
            _mockRepository.Verify(repo => repo.DeleteById(id), Times.Once);
        }
}
