namespace BlogAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Repositories;

[Route("api/[controller]")]
[ApiController]
public class PostController(IPostRepository repository) : ControllerBase
{
    // GET: api/Post
    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetPosts()
    {
        return await repository.Get();
    }

    // GET: api/Post/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var comment = await repository.GetById(id);
        return comment == null ? NotFound() : comment;
    }

    // POST: api/Post
    [HttpPost]
    public async Task<ActionResult<Post>> PostPost(Post comment)
    {
        await repository.Insert(comment);
        return CreatedAtAction(nameof(GetPost), new { id = comment.ID }, comment);
    }

    // PUT: api/Post/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutPost(int id, Post comment)
    {
        if (id != comment.ID) return BadRequest();
        return await repository.Update(comment) ? NoContent() : NotFound();
    }

    // DELETE: api/Post/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        return await repository.DeleteById(id) ? NoContent() : NotFound();
    }
}