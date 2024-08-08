using Moq;                                  //For Setup, Verify, ReturnsAsync
using Microsoft.AspNetCore.Mvc;             //For ActionResult, CreatedAtActionResult, etc.
using BlogAPI.Controllers;                  //TagController.cs is in here
using BlogAPI.Models;                       //Tag.cs is in here
using BlogAPI.Repositories;                 //ITagRepository.cs is in here

namespace BlogAPITests;

public class TagControllerTests
{
    //Using a mock repository to isolate unit tests from actual database
    private readonly Mock<ITagRepository> _mockRepository;
    //Instance of class we are testing
    private readonly TagController _controller;

    public TagControllerTests()
    {
        //Initializing mock repository
        _mockRepository = new Mock<ITagRepository>();
        //Passing mock repository object to TagController constructor
        _controller = new TagController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetTags_ReturnsAllTags()
    {
        //ARRANGE
        //Creating a list of tags, and initializing it with 2 tags
        List<Tag> tags = new List<Tag>
        {
            new Tag { ID = 1, Name = "Tag1" },
            new Tag { ID = 2, Name = "Tag2" }
        };

        //Configuring behavior of _mockRepository, returning tag list asynchronously 
        _mockRepository.Setup(repo => repo.Get()).ReturnsAsync(tags);

        //ACT
        //Calling GetTags() method
        var result = await _controller.GetTags();

        //ASSERT
        //Verifying result is of type ActionResult<List<Tag>>
        var actionResult = Assert.IsType<ActionResult<List<Tag>>>(result);
        //Verifying that the value of actionResult is List<Tag>
        var returnValue = Assert.IsType<List<Tag>>(actionResult.Value);
        //Verifying that returnValue has a count of 2
        Assert.Equal(2, returnValue.Count);
    }

    [Theory]
    [InlineData(1, "Tag1")]
    [InlineData(2, "Tag2")]
    public async Task GetTag_ReturnsTagById(int id, string name)
    {
        //ARRANGE
        //Creating a tag object with parameters
        Tag tag = new Tag { ID = id, Name = name };
        //Configuring behavior of _mockRepository, return tag asynchronously
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(tag);

        //ACT
        //Calling GetTag(int id) method
        var result = await _controller.GetTag(id);

        //ASSERT
        //Verifying result is of type ActionResult<Tag>
        var actionResult = Assert.IsType<ActionResult<Tag>>(result);
        //Verifying that the value of actionResult is Tag
        var returnValue = Assert.IsType<Tag>(actionResult.Value);
        //Verifying that the returnValue properties match the values from the Tag object
        Assert.Equal(tag.ID, returnValue.ID);
        Assert.Equal(tag.Name, returnValue.Name);
    }

    [Theory]
    [InlineData(1, "NewTag1")]
    [InlineData(2, "NewTag2")]
    public async Task PostTag_CreatesNewTag(int id, string name)
    {
        //ARRANGE
        //Creating a tag object with parameters
        Tag tag = new Tag { ID = id, Name = name };
        //Configuring behavior of _mockRepository. Task.CompletedTask indicates successful operation
        _mockRepository.Setup(repo => repo.Insert(tag)).Returns(Task.CompletedTask);

        //ACT
        //Calling PostTag(Tag tag) method
        var result = await _controller.PostTag(tag);

        //ASSERT
        //Verifying result is of type ActionResult<Tag>
        var actionResult = Assert.IsType<ActionResult<Tag>>(result);
        //Verifying that Result property of actionResult is of type CreatedAtActionResult
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //Verifying that Value property of createdAtActionResult is of type Tag
        var returnValue = Assert.IsType<Tag>(createdAtActionResult.Value);
        //Verifying that Name of returnValue matches Name of Tag object
        Assert.Equal(tag.Name, returnValue.Name);
    }

    [Theory]
    [InlineData(1, "UpdatedTag1")]
    [InlineData(2, "UpdatedTag2")]
    public async Task PutTag_UpdatesExistingTag(int id, string name)
    {
        //ARRANGE
        //Creating objects for an "existing" tag, and a tag that will represent the changes to be made
        Tag existingTag = new Tag { ID = id, Name = $"Tag{id}" };
        Tag updatedTag = new Tag { ID = id, Name = name };

        //Configuring behavior of _mockRepository. Return existingTag asynchronously and true indicating successful update
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(existingTag);
        _mockRepository.Setup(repo => repo.Update(updatedTag)).ReturnsAsync(true);

        //ACT
        //Calling PutTag(int id, Tag tag) method
        var result = await _controller.PutTag(id, updatedTag);

        //ASSERT
        //Verifying that the result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the Update method was called exactly once
        _mockRepository.Verify(repo => repo.Update(updatedTag), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteTag_DeletesExistingTag(int id)
    {
        //ARRANGE
        //Configuring behavior of _mockRepository. Return true indicating successful delete
        _mockRepository.Setup(repo => repo.DeleteById(id)).ReturnsAsync(true);

        //ACT
        //Calling DeleteTag(int id) method
        var result = await _controller.DeleteTag(id);

        //ASSERT
        //Verifying that result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the DeleteById method was called exactly once
        _mockRepository.Verify(repo => repo.DeleteById(id), Times.Once);
    }
}