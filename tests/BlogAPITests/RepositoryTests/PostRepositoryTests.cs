using Microsoft.EntityFrameworkCore;         //For DbContextOptions, InMemoryDbContextOptionsBuilder
using BlogAPI.Models;                        //For BlogContext, Post, Tag, Category, User, PostTag
using BlogAPI.Repositories;                  //For PostRepository

namespace BlogAPITests;

public class PostRepositoryTests
{
    //Field to hold configuration options
    private readonly DbContextOptions<BlogContext> _contextOptions;

    public PostRepositoryTests()
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
    public async Task GetAll_ReturnsAllPosts()
    {
        //ARRANGE
        //Creating 2 posts, then adding to context
        using var context = CreateContext();
        context.Posts.AddRange(
            new Post 
            { 
                Topic = "Post1", 
                Body = "Body1", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                Category = new Category { Name = "Category1" }, 
                Poster = new User { Username = "User1", Password = "Password1" } 
            },
            new Post 
            { 
                Topic = "Post2", 
                Body = "Body2", 
                CreatedAt = DateTime.Now, 
                EditedAt = DateTime.Now, 
                Category = new Category { Name = "Category2" }, 
                Poster = new User { Username = "User2", Password = "Password2" } 
            }
        );
        await context.SaveChangesAsync();
        var repository = new PostRepository(context);

        //ACT
        //Calling Get() method
        var posts = await repository.Get();

        //ASSERT
        //Verifying there are 2 posts
        Assert.Equal(2, posts.Count());
    }

    [Fact]
    public async Task GetById_ReturnsCorrectPost()
    {
        //ARRANGE
        //Creating 1 post, then adding to context
        using var context = CreateContext();
        context.Posts.Add(new Post 
        { 
            ID = 1, 
            Topic = "Post1", 
            Body = "Body1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            Category = new Category { Name = "Category1" }, 
            Poster = new User { Username = "User1", Password = "Password1" } 
        });
        await context.SaveChangesAsync();
        var repository = new PostRepository(context);

        //ACT
        //Calling GetById(object id) method
        var post = await repository.GetById(1);

        //ASSERT
        //Verifying that post returned is not null, and that it equals the post we created
        Assert.NotNull(post);
        Assert.Equal("Post1", post.Topic);
    }

    [Fact]
    public async Task Insert_AddsPost()
    {
        //ARRANGE
        using var context = CreateContext();
        var repository = new PostRepository(context);

        //ACT
        var post = new Post 
        { 
            Topic = "NewPost", 
            Body = "NewBody", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            Category = new Category { Name = "NewCategory" }, 
            Poster = new User { Username = "NewUser", Password = "NewPassword" } 
        };
        //Calling Insert(TEntity entity)
        await repository.Insert(post);
        await context.SaveChangesAsync();

        //ASSERT
        //Verifying that there is only 1 post, and it is the post we created
        var posts = await context.Posts.ToListAsync();
        Assert.Single(posts);
        Assert.Equal("NewPost", posts[0].Topic);
    }

    [Fact]
    public async Task Update_UpdatesPost()
    {
        //ARRANGE
        //Creating post then adding to context
        using var context = CreateContext();
        var post = new Post 
        { 
            ID = 1, 
            Topic = "Post1", 
            Body = "Body1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            Category = new Category { Name = "Category1" }, 
            Poster = new User { Username = "User1", Password = "Password1" } 
        };
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        var repository = new PostRepository(context);

        //ACT
        //Getting post by id, detaching it, changing topic, calling Update(TEntity entity), then saving changes
        context.Entry(post).State = EntityState.Detached;
        post.Topic = "UpdatedPost";
        await repository.Update(post);
        await context.SaveChangesAsync();

        //ASSERT
        //Making sure post we created and updated exists as we expect
        var updatedPost = await context.Posts.FindAsync(1);
        Assert.Equal("UpdatedPost", updatedPost.Topic);
    }


    [Fact]
    public async Task Delete_RemovesPost()
    {
        //ARRANGE
        //Creating a post then adding to context
        using var context = CreateContext();
        context.Posts.Add(new Post 
        { 
            ID = 1, 
            Topic = "Post1", 
            Body = "Body1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now,
            Category = new Category { Name = "Category1" }, 
            Poster = new User { Username = "User1", Password = "Password1" } 
        });
        await context.SaveChangesAsync();
        var repository = new PostRepository(context);

        //ACT
        //Calling DeleteById(object id) and saving changes
        await repository.DeleteById(1);
        await context.SaveChangesAsync();

        //ASSERT
        //Checking if context is empty as we expect
        var posts = await context.Posts.ToListAsync();
        Assert.Empty(posts);
    }

    [Fact]
    public async Task GetTagsByPostID_ReturnsTagsForPost()
    {
        //ARRANGE
        //Creating a post and tags, then adding to context
        using var context = CreateContext();
        var post = new Post 
        { 
            ID = 1, 
            Topic = "Post1", 
            Body = "Body1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            Category = new Category { Name = "Category1" }, 
            Poster = new User { Username = "User1", Password = "Password1" } 
        };
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        var tag1 = new Tag { ID = 1, Name = "Tag1" };
        var tag2 = new Tag { ID = 2, Name = "Tag2" };
        context.Tags.AddRange(tag1, tag2);

        context.PostTags.AddRange(
            new PostTag { PostID = post.ID, TagID = tag1.ID },
            new PostTag { PostID = post.ID, TagID = tag2.ID }
        );
        await context.SaveChangesAsync();
        var repository = new PostRepository(context);

        //ACT
        //Calling GetTagsByPostID(int postID) method
        var tags = await repository.GetTagsByPostID(post.ID);

        //ASSERT
        //Verifying that there are 2 tags and they belong to the post
        Assert.NotNull(tags);
        Assert.Equal(2, tags.Count);
        Assert.Contains(tags, t => t.Name == "Tag1");
        Assert.Contains(tags, t => t.Name == "Tag2");
    }
}