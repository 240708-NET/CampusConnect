namespace BlogAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Repositories;

[Route("api/[controller]")]
[ApiController]
public class CommentController(ICommentRepository repository) : ControllerBase
{
    // GET: api/Comment
    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetComments()
    {
        return await repository.Get();
    }

    // GET: api/Comment/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        var comment = await repository.GetById(id);
        return comment is null ? NotFound() : comment;
    }

    // GET: api/Comment/{id}/ChildComments
    [HttpGet("{id}/ChildComments")]
    public async Task<ActionResult<List<Comment>>> GetChildComments(int id)
    {
        var comments = await repository.GetChildCommentsByParentID(id);
        return comments is null ? NotFound() : comments;
    }

    // POST: api/Comment
    [HttpPost]
    public async Task<ActionResult<Comment>> PostComment(Comment comment)
    {
        await repository.Insert(comment);
        return CreatedAtAction(nameof(GetComment), new { id = comment.ID }, comment);
    }

    // PUT: api/Comment/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutComment(int id, Comment comment)
    {
        if (id != comment.ID) return BadRequest();
        return await repository.Update(comment) ? NoContent() : NotFound();
    }

    // DELETE: api/Comment/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        return await repository.DeleteById(id) ? NoContent() : NotFound();
    }
}
