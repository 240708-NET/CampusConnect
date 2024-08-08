using Moq;                                  //For Setup, Verify, ReturnsAsync
using Microsoft.AspNetCore.Mvc;             //For ActionResult, CreatedAtActionResult, etc.
using BlogAPI.Controllers;                  //PostController.cs is in here
using BlogAPI.Models;                       //Post.cs is in here
using BlogAPI.Repositories;                 //IPostRepository.cs is in here

namespace BlogAPITests;

public class PostControllerTests
{
    //Using a mock repository to isolate unit tests from actual database
    private readonly Mock<IPostRepository> _mockRepository;
    //Instance of class we are testing
    private readonly PostController _controller;

    public PostControllerTests()
    {
        //Initializing mock repository
        _mockRepository = new Mock<IPostRepository>();
        //Passing mock repository object to PostController constructor
        _controller = new PostController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetPosts_ReturnsAllPosts()
    {
        //ARRANGE
        //Creating a list of posts, and initializing it with 2 posts
        List<Post> posts = new List<Post>
        {
            new Post { ID = 1, Topic = "Topic1", Body = "Body1", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, PostCategory = new Category { ID = 1, Name = "Category1" }, Poster = new User { ID = 1, Username = "User1", Password = "Password1" }, Tags = new List<Tag>() },
            new Post { ID = 2, Topic = "Topic2", Body = "Body2", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, PostCategory = new Category { ID = 2, Name = "Category2" }, Poster = new User { ID = 2, Username = "User2", Password = "Password2" }, Tags = new List<Tag>() }
        };

        //Configuring behavior of _mockRepository, returning post list asynchronously 
        _mockRepository.Setup(repo => repo.Get()).ReturnsAsync(posts);

        //ACT
        //Calling GetPosts() method
        var result = await _controller.GetPosts();

        //ASSERT
        //Verifying result is of type ActionResult<List<Post>>
        var actionResult = Assert.IsType<ActionResult<List<Post>>>(result);
        //Verifying that the value of actionResult is List<Post>
        var returnValue = Assert.IsType<List<Post>>(actionResult.Value);
        //Verifying that returnValue has a count of 2
        Assert.Equal(2, returnValue.Count);
    }

    [Theory]
    [InlineData(1, "Post1")]
    [InlineData(2, "Post2")]
    public async Task GetPost_ReturnsPostById(int id, string title)
    {
        //ARRANGE
        //Creating a post object with parameters
        Post post = new Post { ID = id, Topic = title, Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, PostCategory = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, Tags = new List<Tag>() };
        //Configuring behavior of _mockRepository, return post asynchronously
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(post);

        //ACT
        //Calling GetPost(int id) method
        var result = await _controller.GetPost(id);

        //ASSERT
        //Verifying result is of type ActionResult<Post>
        var actionResult = Assert.IsType<ActionResult<Post>>(result);
        //Verifying that the value of actionResult is Post
        var returnValue = Assert.IsType<Post>(actionResult.Value);
        //Verifying that the returnValue properties match the values from the Post object
        Assert.Equal(post.ID, returnValue.ID);
        Assert.Equal(post.Topic, returnValue.Topic);
    }

    [Theory]
    [InlineData(1, "NewPost1")]
    [InlineData(2, "NewPost2")]
    public async Task PostPost_CreatesNewPost(int id, string title)
    {
        //ARRANGE
        //Creating a post object with parameters
        Post post = new Post { ID = id, Topic = title, Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, PostCategory = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, Tags = new List<Tag>() };
        //Configuring behavior of _mockRepository. Task.CompletedTask indicates successful operation
        _mockRepository.Setup(repo => repo.Insert(post)).Returns(Task.CompletedTask);

        //ACT
        //Calling PostPost(Post post) method
        var result = await _controller.PostPost(post);

        //ASSERT
        //Verifying result is of type ActionResult<Post>
        var actionResult = Assert.IsType<ActionResult<Post>>(result);
        //Verifying that Result property of actionResult is of type CreatedAtActionResult
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //Verifying that Value property of createdAtActionResult is of type Post
        var returnValue = Assert.IsType<Post>(createdAtActionResult.Value);
        //Verifying that Topic of returnValue matches Topic of Post object
        Assert.Equal(post.Topic, returnValue.Topic);
    }

    [Theory]
    [InlineData(1, "UpdatedPost1")]
    [InlineData(2, "UpdatedPost2")]
    public async Task PutPost_UpdatesExistingPost(int id, string title)
    {
        //ARRANGE
        //Creating objects for an "existing" post, and a post that will represent the changes to be made
        Post existingPost = new Post { ID = id, Topic = $"Post{id}", Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, PostCategory = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, Tags = new List<Tag>() };
        Post updatedPost = new Post { ID = id, Topic = title, Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, PostCategory = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, Tags = new List<Tag>() };

        //Configuring behavior of _mockRepository. Return existingPost asynchronously and true indicating successful update
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(existingPost);
        _mockRepository.Setup(repo => repo.Update(updatedPost)).ReturnsAsync(true);

        //ACT
        //Calling PutPost(int id, Post post) method
        var result = await _controller.PutPost(id, updatedPost);

        //ASSERT
        //Verifying that the result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the Update method was called exactly once
        _mockRepository.Verify(repo => repo.Update(updatedPost), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeletePost_DeletesExistingPost(int id)
    {
        //ARRANGE
        //Configuring behavior of _mockRepository. Return true indicating successful delete
        _mockRepository.Setup(repo => repo.DeleteById(id)).ReturnsAsync(true);

        //ACT
        //Calling DeletePost(int id) method
        var result = await _controller.DeletePost(id);

        //ASSERT
        //Verifying that result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the DeleteById method was called exactly once
        _mockRepository.Verify(repo => repo.DeleteById(id), Times.Once);
    }
}