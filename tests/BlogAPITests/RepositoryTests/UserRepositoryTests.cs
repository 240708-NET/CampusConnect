using Microsoft.EntityFrameworkCore;         //For DbContextOptions, InMemoryDbContextOptionsBuilder
using BlogAPI.Models;                        //For BlogContext, User, Post, Comment
using BlogAPI.Repositories;                  //For UserRepository

namespace BlogAPITests;

public class UserRepositoryTests
{
    //Field to hold configuration options
    private readonly DbContextOptions<BlogContext> _contextOptions;

    public UserRepositoryTests()
    {
        //For options, use in-memory DB, and ensure a new DB for each test
        _contextOptions = new DbContextOptionsBuilder<BlogContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    //Method to create a new BlogContext object
    private BlogContext CreateContext() 
    {
        return new BlogContext(_contextOptions);
    }

    [Fact]
    public async Task GetAll_ReturnsAllUsers()
    {
        //ARRANGE
        //Creating 2 users, then adding to context
        using var context = CreateContext();
        context.Users.AddRange(
            new User { Username = "User1", Password = "Password1" },
            new User { Username = "User2", Password = "Password2" }
        );
        await context.SaveChangesAsync();
        var repository = new UserRepository(context);

        //ACT
        //Calling Get() method
        var users = await repository.Get();

        //ASSERT
        //Verifying there are 2 users
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task GetById_ReturnsCorrectUser()
    {
        //ARRANGE
        //Creating 1 user, then adding to context
        using var context = CreateContext();
        context.Users.Add(new User { ID = 1, Username = "User1", Password = "Password1" });
        await context.SaveChangesAsync();
        var repository = new UserRepository(context);

        //ACT
        //Calling GetById(object id) method
        var user = await repository.GetById(1);

        //ASSERT
        //Verifying that user returned is not null, and that it equals the user we created
        Assert.NotNull(user);
        Assert.Equal("User1", user.Username);
    }

    [Fact]
    public async Task Insert_AddsUser()
    {
        //ARRANGE
        using var context = CreateContext();
        var repository = new UserRepository(context);

        //ACT
        var user = new User { Username = "NewUser", Password = "NewPassword" };
        //Calling Insert(TEntity entity)
        await repository.Insert(user);
        await context.SaveChangesAsync();

        //ASSERT
        //Verifying that there is only 1 user, and it is the user we created
        var users = await context.Users.ToListAsync();
        Assert.Single(users);
        Assert.Equal("NewUser", users[0].Username);
    }

    [Fact]
    public async Task Update_UpdatesUser()
    {
        //ARRANGE
        //Creating user then adding to context
        using var context = CreateContext();
        var user = new User 
        { 
            ID = 1, 
            Username = "User1", 
            Password = "Password1"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);

        //ACT
        //Getting user by id, detaching it, changing username, calling Update(TEntity entity), then saving changes
        context.Entry(user).State = EntityState.Detached;
        user.Username = "UpdatedUser";
        await repository.Update(user);
        await context.SaveChangesAsync();

        //ASSERT
        //Making sure user we created and updated exists as we expect
        var updatedUser = await context.Users.FindAsync(1);
        Assert.Equal("UpdatedUser", updatedUser.Username);
    }

    [Fact]
    public async Task Delete_RemovesUser()
    {
        //ARRANGE
        //Creating a user then adding to context
        using var context = CreateContext();
        context.Users.Add(new User { ID = 1, Username = "User1", Password = "Password1" });
        await context.SaveChangesAsync();
        var repository = new UserRepository(context);

        //ACT
        //Calling DeleteById(object id) and saving changes
        await repository.DeleteById(1);
        await context.SaveChangesAsync();

        //ASSERT
        //Checking if context is empty as we expect
        var users = await context.Users.ToListAsync();
        Assert.Empty(users);
    }

    [Fact]
    public async Task GetPostsByUserID_ReturnsPostsForUser()
    {
        //ARRANGE
        //Creating a user and posts, then adding to context
        using var context = CreateContext();
        var user = new User { ID = 1, Username = "User1", Password = "Password1" };
        context.Users.Add(user);
        context.Posts.AddRange(
            new Post { ID = 1, PosterID = 1, Topic = "Topic1", Body = "Body1" },
            new Post { ID = 2, PosterID = 1, Topic = "Topic2", Body = "Body2" }
        );
        await context.SaveChangesAsync();
        var repository = new UserRepository(context);

        //ACT
        //Calling GetPostsByUserID(int userID) method
        var posts = await repository.GetPostsByUserID(1);

        //ASSERT
        //Verifying that there are 2 posts and they belong to the user
        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count);
    }

    [Fact]
    public async Task GetCommentsByUserID_ReturnsCommentsForUser()
    {
        //ARRANGE
        //Creating a user and comments, then adding to context
        using var context = CreateContext();
        var user = new User { ID = 1, Username = "User1", Password = "Password1" };
        context.Users.Add(user);
        context.Comments.AddRange(
            new Comment { ID = 1, CommenterID = 1, Body = "Body1" },
            new Comment { ID = 2, CommenterID = 1, Body = "Body2" }
        );
        await context.SaveChangesAsync();
        var repository = new UserRepository(context);

        //ACT
        //Calling GetCommentsByUserID(int userID) method
        var comments = await repository.GetCommentsByUserID(1);

        //ASSERT
        //Verifying that there are 2 comments and they belong to the user
        Assert.NotNull(comments);
        Assert.Equal(2, comments.Count);
    }
}
