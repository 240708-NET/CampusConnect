using Xunit;                                //For [Fact], [Theory], etc.
using Moq;                                  //For Setup, Verify, ReturnsAsync
using Microsoft.AspNetCore.Mvc;             //For ActionResult, CreatedAtActionResult, etc.
using Microsoft.EntityFrameworkCore;        //For DbContext
using BlogAPI.Controllers;                  //UserController.cs is in here
using BlogAPI.Models;                       //user and BlogContext are in here

namespace BlogAPITests;

public class UserControllerTests
{
    //Using a mock context to isolate unit tests from actual database
    private readonly Mock<BlogContext> _mockContext;
    //Instance of class we are testing
    private readonly UserController _controller;

    public UserControllerTests()
    {
        //Configuring options for BlogContext. Use in-memory DB so we don't have to use a "real" one
        DbContextOptionsBuilder<BlogContext> options = new DbContextOptionsBuilder<BlogContext>()
            .UseInMemoryDatabase(databaseName: "BlogTestDb")
            .Options;

        //Initializing mock context with options we just configured
        _mockContext = new Mock<BlogContext>(options);
        //Passing BlogContext object to UserController constructor, .Object to make sure we don't pass Mock<BlogContext>
        _controller = new UserController(_mockContext.Object);
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

        //Configuring behavior of _mockContext, returning user list asynchronously 
        _mockContext.Setup(x => x.Users.ToListAsync())
            .ReturnsAsync(users);

        //ACT
        //Calling GetUsers() method
        ActionResult<IEnumerable<User>> result = await _controller.GetUsers();

        //ASSERT
        //Verifying result is of type ActionResult<IEnumerable<User>>
        ActionResult<IEnumerable<User>> actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
        //Verifying that the value of actionResult is List<User>.
        List<User> returnValue = Assert.IsType<List<User>>(actionResult.Value);
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
        //Configuring behavior of _mockContext, return user asynchronously
        _mockContext.Setup(x => x.Users.FindAsync(id)).ReturnsAsync(user);

        //ACT
        //Calling GetUser(long id) method
        ActionResult<User> result = await _controller.GetUser(id);

        //ASSERT
        //Verifying result is of type ActionResult<User>>
        ActionResult<User> actionResult = Assert.IsType<ActionResult<User>>(result);
        //Verifying that the value of actionResult is User
        User returnValue = Assert.IsType<User>(actionResult.Value);
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
        //Coonfiguring behavior of_mockContext. 1 will be returned asynchronously, indicating 1 change was saved
        _mockContext.Setup(x => x.Users.Add(user));
        _mockContext.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        //ACT
        //Calling PostUser(User user) method
        ActionResult<User> result = await _controller.PostUser(user);

        //ASSERT
        //Verifying result is of type ActionResult<User>>
        actionResult<User> actionResult = Assert.IsType<ActionResult<User>>(result);
        //Verifying that Result property of actionResult is of type CreatedAtActionResult
        CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //Verifying that Value property of createdAtActionResult is of type User
        User returnValue = Assert.IsType<User>(createdAtActionResult.Value);
        //Verifying that Username of returnValue matches Username of User object
        Assert.Equal(user.Username, returnValue.Username);
    }

    [Theory]
    [InlineData(1, "UpdatedUser1", "UpdatedPassword1")]
    [InlineData(2, "UpdatedUser2", "UpdatedPassword2")]
    public async Task PutUser_UpdatesExistingUser(int id, string username, string password)
    {
        //ARRANGE
        //Creating objects for an "existing" user, and a user that will represent that changes to be made
        User existingUser = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" };
        User updatedUser = new User { ID = id, Username = username, Password = password };

        //Configuring behavior of _mockContext. Return existingUser asynchronously and when
        //SaveChangesAsync is called, return 1, indicating 1 save was changed
        _mockContext.Setup(x => x.Users.FindAsync(id)).ReturnsAsync(existingUser);
        _mockContext.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        //ACT
        //Calling PutUser(long id, User user) method
        ActionResult result = await _controller.PutUser(id, updatedUser);

        //ASSERT
        //Verifying that the result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the SaveChangesAsync method was called exactly once
        _mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
        //Verifyiing that the username and password of the existing user have the vvalues from the updated user
        Assert.Equal(username, existingUser.Username);
        Assert.Equal(password, existingUser.Password);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteUser_DeletesExistingUser(int id)
    {
        //ARRANGE
        //Creating a User object
        User existingUser = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" };

        //Configuring behavior of _mockContext. Return existingUser asynchronously and when
        //SaveChangesAsync is called, return 1, indicating 1 save was changed
        _mockContext.Setup(x => x.Users.FindAsync(id)).ReturnsAsync(existingUser);
        _mockContext.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        //ACT
        //Calling DeleteUser(long id) method
        ActionResult<User> result = await _controller.DeleteUser(id);

        //ASSERT
        //Verifying that result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the Remove method on the Users DbSet was called exactly once, with existingUser as parameter
        _mockContext.Verify(x => x.Users.Remove(existingUser), Times.Once);
        //Verifying that the SaveChangesAsync method was called exactly once
        _mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}