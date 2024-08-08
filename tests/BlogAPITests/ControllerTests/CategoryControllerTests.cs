using Moq;                                  //For Setup, Verify, ReturnsAsync
using Microsoft.AspNetCore.Mvc;             //For ActionResult, CreatedAtActionResult, etc.
using BlogAPI.Controllers;                  //CategoryController.cs is in here
using BlogAPI.Models;                       //Category.cs is in here
using BlogAPI.Repositories;                 //ICategoryRepository.cs is in here

namespace BlogAPITests;

public class CategoryControllerTests
{
    //Using a mock repository to isolate unit tests from actual database
    private readonly Mock<ICategoryRepository> _mockRepository;
    //Instance of class we are testing
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        //Initializing mock repository
        _mockRepository = new Mock<ICategoryRepository>();
        //Passing mock repository object to CategoryController constructor
        _controller = new CategoryController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetCategories_ReturnsAllCategories()
    {
        //ARRANGE
        //Creating a list of categories, and initializing it with 2 categories
        List<Category> categories = new List<Category>
        {
            new Category { ID = 1, Name = "Category1" },
            new Category { ID = 2, Name = "Category2" }
        };

        //Configuring behavior of _mockRepository, returning category list asynchronously 
        _mockRepository.Setup(repo => repo.Get()).ReturnsAsync(categories);

        //ACT
        //Calling GetCategories() method
        var result = await _controller.GetCategories();

        //ASSERT
        //Verifying result is of type ActionResult<List<Category>>
        var actionResult = Assert.IsType<ActionResult<List<Category>>>(result);
        //Verifying that the value of actionResult is List<Category>
        var returnValue = Assert.IsType<List<Category>>(actionResult.Value);
        //Verifying that returnValue has a count of 2
        Assert.Equal(2, returnValue.Count);
    }

    [Theory]
    [InlineData(1, "Category1")]
    [InlineData(2, "Category2")]
    public async Task GetCategory_ReturnsCategoryById(int id, string name)
    {
        //ARRANGE
        //Creating a category object with parameters
        Category category = new Category { ID = id, Name = name };
        //Configuring behavior of _mockRepository, return category asynchronously
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(category);

        //ACT
        //Calling GetCategory(int id) method
        var result = await _controller.GetCategory(id);

        //ASSERT
        //Verifying result is of type ActionResult<Category>
        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        //Verifying that the value of actionResult is Category
        var returnValue = Assert.IsType<Category>(actionResult.Value);
        //Verifying that the returnValue properties match the values from the Category object
        Assert.Equal(category.ID, returnValue.ID);
        Assert.Equal(category.Name, returnValue.Name);
    }

    [Theory]
    [InlineData(1, "NewCategory1")]
    [InlineData(2, "NewCategory2")]
    public async Task PostCategory_CreatesNewCategory(int id, string name)
    {
        //ARRANGE
        //Creating a category object with parameters
        Category category = new Category { ID = id, Name = name };
        //Configuring behavior of _mockRepository. Task.CompletedTask indicates successful operation
        _mockRepository.Setup(repo => repo.Insert(category)).Returns(Task.CompletedTask);

        //ACT
        //Calling PostCategory(Category category) method
        var result = await _controller.PostCategory(category);

        //ASSERT
        //Verifying result is of type ActionResult<Category>
        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        //Verifying that Result property of actionResult is of type CreatedAtActionResult
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //Verifying that Value property of createdAtActionResult is of type Category
        var returnValue = Assert.IsType<Category>(createdAtActionResult.Value);
        //Verifying that Name of returnValue matches Name of Category object
        Assert.Equal(category.Name, returnValue.Name);
    }

    [Theory]
    [InlineData(1, "UpdatedCategory1")]
    [InlineData(2, "UpdatedCategory2")]
    public async Task PutCategory_UpdatesExistingCategory(int id, string name)
    {
        //ARRANGE
        //Creating objects for an "existing" category, and a category that will represent the changes to be made
        Category existingCategory = new Category { ID = id, Name = $"Category{id}" };
        Category updatedCategory = new Category { ID = id, Name = name };

        //Configuring behavior of _mockRepository. Return existingCategory asynchronously and true indicating successful update
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(existingCategory);
        _mockRepository.Setup(repo => repo.Update(updatedCategory)).ReturnsAsync(true);

        //ACT
        //Calling PutCategory(int id, Category category) method
        var result = await _controller.PutCategory(id, updatedCategory);

        //ASSERT
        //Verifying that the result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the Update method was called exactly once
        _mockRepository.Verify(repo => repo.Update(updatedCategory), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteCategory_DeletesExistingCategory(int id)
    {
        //ARRANGE
        //Configuring behavior of _mockRepository. Return true indicating successful delete
        _mockRepository.Setup(repo => repo.DeleteById(id)).ReturnsAsync(true);

        //ACT
        //Calling DeleteCategory(int id) method
        var result = await _controller.DeleteCategory(id);

        //ASSERT
        //Verifying that result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the DeleteById method was called exactly once
        _mockRepository.Verify(repo => repo.DeleteById(id), Times.Once);
    }
}
