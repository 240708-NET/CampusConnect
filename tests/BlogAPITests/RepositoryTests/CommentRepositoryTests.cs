using Microsoft.EntityFrameworkCore;         //For DbContextOptions, InMemoryDbContextOptionsBuilder
using BlogAPI.Models;                        //For BlogContext, Comment
using BlogAPI.Repositories;                  //For CommentRepository

namespace BlogAPITests;

public class CommentRepositoryTests
{
    //Field to hold configuration options
    private readonly DbContextOptions<BlogContext> _contextOptions;

    public CommentRepositoryTests()
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
    public async Task GetAll_ReturnsAllComments()
    {
        //ARRANGE
        //Creating 2 comments, then adding to context
        using var context = CreateContext();
        context.Comments.AddRange(
            new Comment 
            { 
                Body = "Comment1", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                OriginalPost = new Post 
                { 
                    Topic = "Post1", 
                    Body = "Body1", 
                    CreatedAt = DateTime.Now, 
                    EditedAt = DateTime.Now, 
                    PostCategory = new Category { Name = "Category1" }, 
                    Poster = new User { Username = "User1", Password = "Password1" } 
                }, 
                Commenter = new User { Username = "User1", Password = "Password1" }, 
                ParentComment = null 
            },
            new Comment 
            { 
                Body = "Comment2", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                OriginalPost = new Post 
                { 
                    Topic = "Post2", 
                    Body = "Body2", 
                    CreatedAt = DateTime.Now, 
                    EditedAt = DateTime.Now, 
                    PostCategory = new Category { Name = "Category2" }, 
                    Poster = new User { Username = "User2", Password = "Password2" } 
                }, 
                Commenter = new User { Username = "User2", Password = "Password2" }, 
                ParentComment = null 
            }
        );
        await context.SaveChangesAsync();
        var repository = new CommentRepository(context);

        //ACT
        //Calling Get() method
        var comments = await repository.Get();

        //ASSERT
        //Verifying there are 2 comments
        Assert.Equal(2, comments.Count());
    }

    [Fact]
    public async Task GetById_ReturnsCorrectComment()
    {
        //ARRANGE
        //Creating 1 comment, then adding to context
        using var context = CreateContext();
        context.Comments.Add(new Comment 
        { 
            ID = 1, 
            Body = "Comment1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            OriginalPost = new Post 
            { 
                Topic = "Post1", 
                Body = "Body1", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                PostCategory = new Category { Name = "Category1" }, 
                Poster = new User { Username = "User1", Password = "Password1" } 
            }, 
            Commenter = new User { Username = "User1", Password = "Password1" }, 
            ParentComment = null 
        });
        await context.SaveChangesAsync();
        var repository = new CommentRepository(context);

        //ACT
        //Calling GetById(object id) method
        var comment = await repository.GetById(1);

        //ASSERT
        //Verifying that comment returned is not null, and that it equals the comment we created
        Assert.NotNull(comment);
        Assert.Equal("Comment1", comment.Body);
    }

    [Fact]
    public async Task Insert_AddsComment()
    {
        //ARRANGE
        using var context = CreateContext();
        var repository = new CommentRepository(context);

        //ACT
        var comment = new Comment 
        { 
            Body = "NewComment", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            OriginalPost = new Post 
            { 
                Topic = "Post1", 
                Body = "Body1", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                PostCategory = new Category { Name = "Category1" }, 
                Poster = new User { Username = "User1", Password = "Password1" } 
            }, 
            Commenter = new User { Username = "User1", Password = "Password1" }, 
            ParentComment = null 
        };
        //Calling Insert(TEntity entity)
        await repository.Insert(comment);
        await context.SaveChangesAsync();

        //ASSERT
        //Verifying that there is only 1 comment, and it is the comment we created
        var comments = await context.Comments.ToListAsync();
        Assert.Single(comments);
        Assert.Equal("NewComment", comments[0].Body);
    }

    [Fact]
    public async Task Update_UpdatesComment()
    {
        //ARRANGE
        //Creating comment then adding to context
        using var context = CreateContext();
        context.Comments.Add(new Comment 
        { 
            ID = 1, 
            Body = "Comment1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            OriginalPost = new Post 
            { 
                Topic = "Post1", 
                Body = "Body1", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                PostCategory = new Category { Name = "Category1" }, 
                Poster = new User { Username = "User1", Password = "Password1" } 
            }, 
            Commenter = new User { Username = "User1", Password = "Password1" }, 
            ParentComment = null 
        });
        await context.SaveChangesAsync();
        var repository = new CommentRepository(context);

        //ACT
        //Getting comment by id, changing body, calling Update(TEntity entity), then saving changes
        var comment = await repository.GetById(1);
        comment.Body = "UpdatedComment";
        await repository.Update(comment);
        await context.SaveChangesAsync();

        //ASSERT
        //Making sure comment we created and updated exists as we expect
        var updatedComment = await context.Comments.FindAsync(1);
        Assert.Equal("UpdatedComment", updatedComment.Body);
    }

    [Fact]
    public async Task Delete_RemovesComment()
    {
        //ARRANGE
        //Creating a comment then adding to context
        using var context = CreateContext();
        context.Comments.Add(new Comment 
        { 
            ID = 1, 
            Body = "Comment1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            OriginalPost = new Post 
            { 
                Topic = "Post1", 
                Body = "Body1", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                PostCategory = new Category { Name = "Category1" }, 
                Poster = new User { Username = "User1", Password = "Password1" } 
            }, 
            Commenter = new User { Username = "User1", Password = "Password1" }, 
            ParentComment = null 
        });
        await context.SaveChangesAsync();
        var repository = new CommentRepository(context);

        //ACT
        //Calling DeleteById(object id) and saving changes
        await repository.DeleteById(1);
        await context.SaveChangesAsync();

        //ASSERT
        //Checking if context is empty as we expect
        var comments = await context.Comments.ToListAsync();
        Assert.Empty(comments);
    }
}
