using Microsoft.EntityFrameworkCore;         //For DbContextOptions, InMemoryDbContextOptionsBuilder
using BlogAPI.Models;                        //For BlogContext, Category, Post
using BlogAPI.Repositories;                  //For CategoryRepository

namespace BlogAPITests;

public class CategoryRepositoryTests
{
    //Field to hold configuration options
    private readonly DbContextOptions<BlogContext> _contextOptions;

    public CategoryRepositoryTests()
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
    public async Task GetAll_ReturnsAllCategories()
    {
        //ARRANGE
        //Creating 2 categories, then adding to context
        using var context = CreateContext();
        context.Categories.AddRange(
            new Category { Name = "Category1" },
            new Category { Name = "Category2" }
        );
        await context.SaveChangesAsync();
        var repository = new CategoryRepository(context);

        //ACT
        //Calling Get() method
        var categories = await repository.Get();

        //ASSERT
        //Verifying there are 2 categories
        Assert.Equal(2, categories.Count());
    }

    [Fact]
    public async Task GetById_ReturnsCorrectCategory()
    {
        //ARRANGE
        //Creating 1 category, then adding to context
        using var context = CreateContext();
        context.Categories.Add(new Category { ID = 1, Name = "Category1" });
        await context.SaveChangesAsync();
        var repository = new CategoryRepository(context);

        //ACT
        //Calling GetById(object id) method
        var category = await repository.GetById(1);

        //ASSERT
        //Verifying that category returned is not null, and that it equals the category we created
        Assert.NotNull(category);
        Assert.Equal("Category1", category.Name);
    }

    [Fact]
    public async Task Insert_AddsCategory()
    {
        //ARRANGE
        using var context = CreateContext();
        var repository = new CategoryRepository(context);

        //ACT
        var category = new Category { Name = "NewCategory" };
        //Calling Insert(TEntity entity)
        await repository.Insert(category);
        await context.SaveChangesAsync();

        //ASSERT
        //Verifying that there is only 1 category, and it is the category we created
        var categories = await context.Categories.ToListAsync();
        Assert.Single(categories);
        Assert.Equal("NewCategory", categories[0].Name);
    }

    [Fact]
    public async Task Update_UpdatesCategory()
    {
        //ARRANGE
        //Creating category then adding to context
        using var context = CreateContext();
        var category = new Category 
        { 
            ID = 1, 
            Name = "Category1"
        };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var repository = new CategoryRepository(context);

        //ACT
        //Getting category by id, detaching it, changing name, calling Update(TEntity entity), then saving changes
        context.Entry(category).State = EntityState.Detached;
        category.Name = "UpdatedCategory";
        await repository.Update(category);
        await context.SaveChangesAsync();

        //ASSERT
        //Making sure category we created and updated exists as we expect
        var updatedCategory = await context.Categories.FindAsync(1);
        Assert.Equal("UpdatedCategory", updatedCategory.Name);
    }

    [Fact]
    public async Task Delete_RemovesCategory()
    {
        //ARRANGE
        //Creating a category then adding to context
        using var context = CreateContext();
        context.Categories.Add(new Category { ID = 1, Name = "Category1" });
        await context.SaveChangesAsync();
        var repository = new CategoryRepository(context);

        //ACT
        //Calling DeleteById(object id) and saving changes
        await repository.DeleteById(1);
        await context.SaveChangesAsync();

        //ASSERT
        //Checking if context is empty as we expect
        var categories = await context.Categories.ToListAsync();
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetPostsByCategoryID_ReturnsPostsForCategory()
    {
        //ARRANGE
        //Creating a category and posts, then adding to context
        using var context = CreateContext();
        var category = new Category { ID = 1, Name = "Category1" };
        context.Categories.Add(category);
        context.Posts.AddRange(
            new Post { ID = 1, CategoryID = 1, Topic = "Topic1", Body = "Body1" },
            new Post { ID = 2, CategoryID = 1, Topic = "Topic2", Body = "Body2" }
        );
        await context.SaveChangesAsync();
        var repository = new CategoryRepository(context);

        //ACT
        //Calling GetPostsByCategoryID(int categoryID) method
        var posts = await repository.GetPostsByCategoryID(1);

        //ASSERT
        //Verifying that there are 2 posts and they belong to the category
        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count);
    }
}