using Microsoft.EntityFrameworkCore;         //For DbContextOptions, InMemoryDbContextOptionsBuilder
using BlogAPI.Models;                        //For BlogContext, PostTag
using BlogAPI.Repositories;                  //For PostTagRepository

namespace BlogAPITests;

public class PostTagRepositoryTests
{
    //Field to hold configuration options
    private readonly DbContextOptions<BlogContext> _contextOptions;

    public PostTagRepositoryTests()
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
    public async Task GetAll_ReturnsAllPostTags()
    {
        //ARRANGE
        //Creating 2 post tags, then adding to context
        using var context = CreateContext();
        context.PostTags.AddRange(
            new PostTag { PostID = 1, TagID = 1 },
            new PostTag { PostID = 2, TagID = 2 }
        );
        await context.SaveChangesAsync();
        var repository = new PostTagRepository(context);

        //ACT
        //Calling Get() method
        var postTags = await repository.Get();

        //ASSERT
        //Verifying there are 2 post tags
        Assert.Equal(2, postTags.Count());
    }

    [Fact]
    public async Task GetById_ReturnsCorrectPostTag()
    {
        //ARRANGE
        //Creating 1 post tag, then adding to context
        using var context = CreateContext();
        context.PostTags.Add(new PostTag { ID = 1, PostID = 1, TagID = 1 });
        await context.SaveChangesAsync();
        var repository = new PostTagRepository(context);

        //ACT
        //Calling GetById(object id) method
        var postTag = await repository.GetById(1);

        //ASSERT
        //Verifying that postTag returned is not null, and that it equals the post tag we created
        Assert.NotNull(postTag);
        Assert.Equal(1, postTag.PostID);
        Assert.Equal(1, postTag.TagID);
    }

    [Fact]
    public async Task Insert_AddsPostTag()
    {
        //ARRANGE
        using var context = CreateContext();
        var repository = new PostTagRepository(context);

        //ACT
        var postTag = new PostTag { PostID = 3, TagID = 3 };
        //Calling Insert(TEntity entity)
        await repository.Insert(postTag);
        await context.SaveChangesAsync();

        //ASSERT
        //Verifying that there is only 1 post tag, and it is the post tag we created
        var postTags = await context.PostTags.ToListAsync();
        Assert.Single(postTags);
        Assert.Equal(3, postTags[0].PostID);
        Assert.Equal(3, postTags[0].TagID);
    }

    [Fact]
    public async Task Update_UpdatesPostTag()
    {
        //ARRANGE
        //Creating post tag then adding to context
        using var context = CreateContext();
        var postTag = new PostTag { ID = 1, PostID = 1, TagID = 1 };
        context.PostTags.Add(postTag);
        await context.SaveChangesAsync();

        var repository = new PostTagRepository(context);

        //ACT
        //Getting post tag by id, detaching it, changing PostID and TagID, calling Update(TEntity entity), then saving changes
        context.Entry(postTag).State = EntityState.Detached;
        postTag.PostID = 2;
        postTag.TagID = 2;
        await repository.Update(postTag);
        await context.SaveChangesAsync();

        //ASSERT
        //Making sure post tag we created and updated exists as we expect
        var updatedPostTag = await context.PostTags.FindAsync(1);
        Assert.Equal(2, updatedPostTag.PostID);
        Assert.Equal(2, updatedPostTag.TagID);
    }

    [Fact]
    public async Task Delete_RemovesPostTag()
    {
        //ARRANGE
        //Creating a post tag then adding to context
        using var context = CreateContext();
        context.PostTags.Add(new PostTag { ID = 1, PostID = 1, TagID = 1 });
        await context.SaveChangesAsync();
        var repository = new PostTagRepository(context);

        //ACT
        //Calling DeleteById(object id) and saving changes
        await repository.DeleteById(1);
        await context.SaveChangesAsync();

        //ASSERT
        //Checking if context is empty as we expect
        var postTags = await context.PostTags.ToListAsync();
        Assert.Empty(postTags);
    }
}