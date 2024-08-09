using Microsoft.EntityFrameworkCore;         //For DbContextOptions, InMemoryDbContextOptionsBuilder
using BlogAPI.Models;                        //For BlogContext, Tag, Post, Category, User, PostTag
using BlogAPI.Repositories;                  //For TagRepository

namespace BlogAPITests;

public class TagRepositoryTests
{
    //Field to hold configuration options
    private readonly DbContextOptions<BlogContext> _contextOptions;

    public TagRepositoryTests()
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
    public async Task GetAll_ReturnsAllTags()
    {
        //ARRANGE
        //Creating 2 tags, then adding to context
        using var context = CreateContext();
        context.Tags.AddRange(
            new Tag { Name = "Tag1" },
            new Tag { Name = "Tag2" }
        );
        await context.SaveChangesAsync();
        var repository = new TagRepository(context);

        //ACT
        //Calling Get() method
        var tags = await repository.Get();

        //ASSERT
        //Verifying there are 2 tags
        Assert.Equal(2, tags.Count());
    }

    [Fact]
    public async Task GetById_ReturnsCorrectTag()
    {
        //ARRANGE
        //Creating 1 tag, then adding to context
        using var context = CreateContext();
        context.Tags.Add(new Tag { ID = 1, Name = "Tag1" });
        await context.SaveChangesAsync();
        var repository = new TagRepository(context);

        //ACT
        //Calling GetById(object id) method
        var tag = await repository.GetById(1);

        //ASSERT
        //Verifying that tag returned is not null, and that it equals the tag we created
        Assert.NotNull(tag);
        Assert.Equal("Tag1", tag.Name);
    }

    [Fact]
    public async Task Insert_AddsTag()
    {
        //ARRANGE
        using var context = CreateContext();
        var repository = new TagRepository(context);

        //ACT
        var tag = new Tag { Name = "NewTag" };
        //Calling Insert(TEntity entity)
        await repository.Insert(tag);
        await context.SaveChangesAsync();

        //ASSERT
        //Verifying that there is only 1 tag, and it is the tag we created
        var tags = await context.Tags.ToListAsync();
        Assert.Single(tags);
        Assert.Equal("NewTag", tags[0].Name);
    }

    [Fact]
    public async Task Update_UpdatesTag()
    {
        //ARRANGE
        //Creating tag then adding to context
        using var context = CreateContext();
        var tag = new Tag 
        { 
            ID = 1, 
            Name = "Tag1"
        };
        context.Tags.Add(tag);
        await context.SaveChangesAsync();

        var repository = new TagRepository(context);

        //ACT
        //Getting tag by id, detaching it, changing name, calling Update(TEntity entity), then saving changes
        context.Entry(tag).State = EntityState.Detached;
        tag.Name = "UpdatedTag";
        await repository.Update(tag);
        await context.SaveChangesAsync();

        //ASSERT
        //Making sure tag we created and updated exists as we expect
        var updatedTag = await context.Tags.FindAsync(1);
        Assert.Equal("UpdatedTag", updatedTag.Name);
    }

    [Fact]
    public async Task Delete_RemovesTag()
    {
        //ARRANGE
        //Creating a tag then adding to context
        using var context = CreateContext();
        context.Tags.Add(new Tag { ID = 1, Name = "Tag1" });
        await context.SaveChangesAsync();
        var repository = new TagRepository(context);

        //ACT
        //Calling DeleteById(object id) and saving changes
        await repository.DeleteById(1);
        await context.SaveChangesAsync();

        //ASSERT
        //Checking if context is empty as we expect
        var tags = await context.Tags.ToListAsync();
        Assert.Empty(tags);
    }

    [Fact]
    public async Task GetPostsByTagID_ReturnsPostsForTag()
    {
        //ARRANGE
        //Creating a tag and posts, then adding to context
        using var context = CreateContext();
        var tag = new Tag 
        { 
            ID = 1, 
            Name = "Tag1" 
        };
        context.Tags.Add(tag);
        await context.SaveChangesAsync();

        var post1 = new Post 
        { 
            ID = 1, 
            Topic = "Post1", 
            Body = "Body1", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            Category = new Category { Name = "Category1" }, 
            Poster = new User { Username = "User1", Password = "Password1" } 
        };
        var post2 = new Post 
        { 
            ID = 2, 
            Topic = "Post2", 
            Body = "Body2", 
            CreatedAt = DateTime.Now, 
            EditedAt = DateTime.Now, 
            Category = new Category { Name = "Category2" }, 
            Poster = new User { Username = "User2", Password = "Password2" } 
        };
        context.Posts.AddRange(post1, post2);

        context.PostTags.AddRange(
            new PostTag { PostID = post1.ID, TagID = tag.ID },
            new PostTag { PostID = post2.ID, TagID = tag.ID }
        );
        await context.SaveChangesAsync();
        var repository = new TagRepository(context);

        //ACT
        //Calling GetPostsByTagID(int tagID) method
        var posts = await repository.GetPostsByTagID(tag.ID);

        //ASSERT
        //Verifying that there are 2 posts and they are associated with the tag
        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count);
        Assert.Contains(posts, p => p.Topic == "Post1");
        Assert.Contains(posts, p => p.Topic == "Post2");
    }
}