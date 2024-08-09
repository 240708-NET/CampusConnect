using Moq;                                  //For Setup, Verify, ReturnsAsync
using Microsoft.AspNetCore.Mvc;             //For ActionResult, CreatedAtActionResult, etc.
using BlogAPI.Controllers;                  //CommentController.cs is in here
using BlogAPI.Models;                       //Comment.cs is in here
using BlogAPI.Repositories;                 //ICommentRepository.cs is in here

namespace BlogAPITests;

public class CommentControllerTests
{
    //Using a mock repository to isolate unit tests from actual database
    private readonly Mock<ICommentRepository> _mockRepository;
    //Instance of class we are testing
    private readonly CommentController _controller;

    public CommentControllerTests()
    {
        //Initializing mock repository
        _mockRepository = new Mock<ICommentRepository>();
        //Passing mock repository object to CommentController constructor
        _controller = new CommentController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetComments_ReturnsAllComments()
    {
        //ARRANGE
        //Creating a list of comments, and initializing it with 2 comments
        List<Comment> comments = new List<Comment>
        {
            new Comment { ID = 1, Body = "Comment1", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, OriginalPost = new Post { ID = 1, Topic = "Post1", Body = "Body1", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, Category = new Category { ID = 1, Name = "Category1" }, Poster = new User { ID = 1, Username = "User1", Password = "Password1" } }, Commenter = new User { ID = 1, Username = "User1", Password = "Password1" }, ParentComment = null },
            new Comment { ID = 2, Body = "Comment2", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, OriginalPost = new Post { ID = 2, Topic = "Post2", Body = "Body2", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, Category = new Category { ID = 2, Name = "Category2" }, Poster = new User { ID = 2, Username = "User2", Password = "Password2" } }, Commenter = new User { ID = 2, Username = "User2", Password = "Password2" }, ParentComment = null }
        };
        comments[0].OriginalPost.Tags.Add(new Tag { Name = "Tag1" });
        comments[1].OriginalPost.Tags.Add(new Tag { Name = "Tag2" });

        //Configuring behavior of _mockRepository, returning comment list asynchronously 
        _mockRepository.Setup(repo => repo.Get()).ReturnsAsync(comments);

        //ACT
        //Calling GetComments() method
        var result = await _controller.GetComments();

        //ASSERT
        //Verifying result is of type ActionResult<List<Comment>>
        var actionResult = Assert.IsType<ActionResult<List<Comment>>>(result);
        //Verifying that the value of actionResult is List<Comment>
        var returnValue = Assert.IsType<List<Comment>>(actionResult.Value);
        //Verifying that returnValue has a count of 2
        Assert.Equal(2, returnValue.Count);
    }

    [Theory]
    [InlineData(1, "Comment1")]
    [InlineData(2, "Comment2")]
    public async Task GetComment_ReturnsCommentById(int id, string content)
    {
        //ARRANGE
        //Creating a comment object with parameters
        Comment comment = new Comment { ID = id, Body = content, CreatedAt = DateTime.Now, EditedAt = DateTime.Now, OriginalPost = new Post { ID = id, Topic = $"Post{id}", Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, Category = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" } }, Commenter = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, ParentComment = null };
        comment.OriginalPost.Tags.Add(new Tag { Name = $"Tag{id}" });

        //Configuring behavior of _mockRepository, return comment asynchronously
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(comment);

        //ACT
        //Calling GetComment(int id) method
        var result = await _controller.GetComment(id);

        //ASSERT
        //Verifying result is of type ActionResult<Comment>
        var actionResult = Assert.IsType<ActionResult<Comment>>(result);
        //Verifying that the value of actionResult is Comment
        var returnValue = Assert.IsType<Comment>(actionResult.Value);
        //Verifying that the returnValue properties match the values from the Comment object
        Assert.Equal(comment.ID, returnValue.ID);
        Assert.Equal(comment.Body, returnValue.Body);
    }

    [Theory]
    [InlineData(1, "NewComment1")]
    [InlineData(2, "NewComment2")]
    public async Task PostComment_CreatesNewComment(int id, string content)
    {
        //ARRANGE
        //Creating a comment object with parameters
        Comment comment = new Comment { ID = id, Body = content, CreatedAt = DateTime.Now, EditedAt = DateTime.Now, OriginalPost = new Post { ID = id, Topic = $"Post{id}", Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, Category = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" } }, Commenter = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, ParentComment = null };
        comment.OriginalPost.Tags.Add(new Tag { Name = $"Tag{id}" });

        //Configuring behavior of _mockRepository. Task.CompletedTask indicates successful operation
        _mockRepository.Setup(repo => repo.Insert(comment)).Returns(Task.CompletedTask);

        //ACT
        //Calling PostComment(Comment comment) method
        var result = await _controller.PostComment(comment);

        //ASSERT
        //Verifying result is of type ActionResult<Comment>
        var actionResult = Assert.IsType<ActionResult<Comment>>(result);
        //Verifying that Result property of actionResult is of type CreatedAtActionResult
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //Verifying that Value property of createdAtActionResult is of type Comment
        var returnValue = Assert.IsType<Comment>(createdAtActionResult.Value);
        //Verifying that Body of returnValue matches Body of Comment object
        Assert.Equal(comment.Body, returnValue.Body);
    }

    [Theory]
    [InlineData(1, "UpdatedComment1")]
    [InlineData(2, "UpdatedComment2")]
    public async Task PutComment_UpdatesExistingComment(int id, string content)
    {
        //ARRANGE
        //Creating objects for an "existing" comment, and a comment that will represent the changes to be made
        Comment existingComment = new Comment { ID = id, Body = $"Comment{id}", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, OriginalPost = new Post { ID = id, Topic = $"Post{id}", Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, Category = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" } }, Commenter = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, ParentComment = null };
        existingComment.OriginalPost.Tags.Add(new Tag { Name = $"Tag{id}" });
        Comment updatedComment = new Comment { ID = id, Body = content, CreatedAt = DateTime.Now, EditedAt = DateTime.Now, OriginalPost = new Post { ID = id, Topic = $"Post{id}", Body = "Body", CreatedAt = DateTime.Now, EditedAt = DateTime.Now, Category = new Category { ID = id, Name = $"Category{id}" }, Poster = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" } }, Commenter = new User { ID = id, Username = $"User{id}", Password = $"Password{id}" }, ParentComment = null };
        updatedComment.OriginalPost.Tags.Add(new Tag { Name = $"Tag{id}" });

        //Configuring behavior of _mockRepository. Return existingComment asynchronously and true indicating successful update
        _mockRepository.Setup(repo => repo.GetById(id)).ReturnsAsync(existingComment);
        _mockRepository.Setup(repo => repo.Update(updatedComment)).ReturnsAsync(true);

        //ACT
        //Calling PutComment(int id, Comment comment) method
        var result = await _controller.PutComment(id, updatedComment);

        //ASSERT
        //Verifying that the result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the Update method was called exactly once
        _mockRepository.Verify(repo => repo.Update(updatedComment), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteComment_DeletesExistingComment(int id)
    {
        //ARRANGE
        //Configuring behavior of _mockRepository. Return true indicating successful delete
        _mockRepository.Setup(repo => repo.DeleteById(id)).ReturnsAsync(true);

        //ACT
        //Calling DeleteComment(int id) method
        var result = await _controller.DeleteComment(id);

        //ASSERT
        //Verifying that result is of type NoContentResult
        Assert.IsType<NoContentResult>(result);
        //Verifying that the DeleteById method was called exactly once
        _mockRepository.Verify(repo => repo.DeleteById(id), Times.Once);
    }
}