using Moq;                                  //For Setup, Verify, ReturnsAsync
using Microsoft.AspNetCore.Mvc;             //For ActionResult, CreatedAtActionResult, etc.
using BlogAPI.Controllers;                  //PostTagController.cs is in here
using BlogAPI.Models;                       //PostTag.cs is in here
using BlogAPI.Repositories;                 //IPostTagRepository.cs is in here

namespace BlogAPITests;

public class PostTagControllerTests
{
    //Using a mock repository to isolate unit tests from actual database
    private readonly Mock<IPostTagRepository> _mockRepository;
    //Instance of class we are testing
    private readonly PostTagController _controller;

    public PostTagControllerTests()
    {
        //Initializing mock repository
        _mockRepository = new Mock<IPostTagRepository>();
        //Passing mock repository object to PostTagController constructor
        _controller = new PostTagController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetPostTags_ReturnsAllPostTags()
    {
        //ARRANGE
        //Creating a list of post tags, and initializing it with 2 post tags
        List<PostTag> postTags = new List<PostTag>
        {
            new PostTag { ID = 1, PostID = 1, TagID = 1 },
            new PostTag { ID = 2, PostID = 2, TagID = 2 }
        };

        //Configuring behavior of _mockRepository, returning post tag list asynchronously 
        _mockRepository.Setup(repo => repo.Get()).ReturnsAsync(postTags);

        //ACT
        //Calling GetPostTags() method
        var result = await _controller.GetPostTags();

        //ASSERT
        //Verifying result is of type ActionResult<List<PostTag>>
        var actionResult = Assert.IsType<ActionResult<List<PostTag>>>(result);
        //Verifying that the value of actionResult is List<PostTag>
        var returnValue = Assert.IsType<List<PostTag>>(actionResult.Value);
        //Verifying that returnValue has a count of 2
        Assert.Equal(2, returnValue.Count);
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 2, 2)]
    public async Task GetPostTag_ReturnsPostTagById(int id, int postId, int tagId)
    {
        //ARRANGE
        //Creating a post tag object with parameters
        PostTag postTag = new PostTag { ID = id, PostID = postId, TagID = tagId };
        //Configuring behavior of _mockRepository, return post tag asynchronously
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(postTag);

        //ACT
        //Calling GetPostTag(int id) method
        var result = await _controller.GetPostTag(id);

        //ASSERT
        //Verifying result is of type ActionResult<PostTag>
        var actionResult = Assert.IsType<ActionResult<PostTag>>(result);
        //Verifying that the value of actionResult is PostTag
        var returnValue = Assert.IsType<PostTag>(actionResult.Value);
        //Verifying that the returnValue properties match the values from the PostTag object
        Assert.Equal(postTag.ID, returnValue.ID);
        Assert.Equal(postTag.PostID, returnValue.PostID);
        Assert.Equal(postTag.TagID, returnValue.TagID);
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 2, 2)]
    public async Task PostPostTag_CreatesNewPostTag(int id, int postId, int tagId)
    {
        //ARRANGE
        //Creating a post tag object with parameters
        PostTag postTag = new PostTag { ID = id, PostID = postId, TagID = tagId };
        //Configuring behavior of _mockRepository. Task.CompletedTask indicates successful operation
        _mockRepository.Setup(repo => repo.Insert(postTag)).Returns(Task.CompletedTask);

        //ACT
        //Calling PostPostTag(PostTag postTag) method
        var result = await _controller.PostPostTag(postTag);

        //ASSERT
        //Verifying result is of type ActionResult<PostTag>
        var actionResult = Assert.IsType<ActionResult<PostTag>>(result);
        //Verifying that Result property of actionResult is of type CreatedAtActionResult
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //Verifying that Value property of createdAtActionResult is of type PostTag
        var returnValue = Assert.IsType<PostTag>(createdAtActionResult.Value);
        //Verifying that PostID and TagID of returnValue match those of PostTag object
        Assert.Equal(postTag.PostID, returnValue.PostID);
        Assert.Equal(postTag.TagID, returnValue.TagID);
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 2, 2)]
    public async Task PutPostTag_UpdatesExistingPostTag(int id, int postId, int tagId)
    {
        //ARRANGE
        //Creating objects for an "existing" post tag, and a post tag that will represent the changes to be made
        PostTag existingPostTag = new PostTag { ID = id, PostID = postId, TagID = tagId };
        PostTag updatedPostTag = new PostTag { ID = id, PostID = postId, TagID = tagId };

        //Configuring behavior of _mockRepository. Return existingPostTag asynchronously and true indicating successful update
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(existingPostTag);
        _mockRepository.Setup(repo => repo.Update(updatedPostTag)).ReturnsAsync(true);

        //ACT
        //Calling PutPostTag(int id, PostTag postTag) method
        var result = await _controller.PutPostTag(id, updatedPostTag);

        //ASSERT
        //Verifying that the result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the Update method was called exactly once
        _mockRepository.Verify(repo => repo.Update(updatedPostTag), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeletePostTag_DeletesExistingPostTag(int id)
    {
        //ARRANGE
        //Configuring behavior of _mockRepository to return true indicating successful delete
        _mockRepository.Setup(repo => repo.DeleteById(id)).ReturnsAsync(true);

        //ACT
        //Calling DeletePostTag(int id) method
        var result = await _controller.DeletePostTag(id);

        //ASSERT
        //Verifying that result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the DeleteById method was called exactly once
        _mockRepository.Verify(repo => repo.DeleteById(id), Times.Once);
    }
}