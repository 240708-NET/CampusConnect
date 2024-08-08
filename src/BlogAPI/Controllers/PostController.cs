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
        var post = await repository.GetById(id);
        return post == null ? NotFound() : post;
    }

    // GET: api/Post/{id}/Tags
    [HttpGet("{id}/Tags")]
    public async Task<ActionResult<List<Tag>>> GetPostTags(int id)
    {
        return await repository.GetTagsById(id);
    }

    // POST: api/Post
    [HttpPost]
    public async Task<ActionResult<Post>> PostPost(Post post)
    {
        await repository.Insert(post);
        return CreatedAtAction(nameof(GetPost), new { id = post.ID }, post);
    }

    // PUT: api/Post/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutPost(int id, Post post)
    {
        if (id != post.ID) return BadRequest();
        return await repository.Update(post) ? NoContent() : NotFound();
    }

    // DELETE: api/Post/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        return await repository.DeleteById(id) ? NoContent() : NotFound();
    }
}