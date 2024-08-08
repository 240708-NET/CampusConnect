namespace BlogAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Repositories;

[Route("api/[controller]")]
[ApiController]
public class PostTagController(IPostTagRepository repository) : ControllerBase
{
    // GET: api/PostTag
    [HttpGet]
    public async Task<ActionResult<List<PostTag>>> GetPostTags()
    {
        return await repository.Get();
    }

    // GET: api/PostTag/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PostTag>> GetPostTag(int id)
    {
        var postTag = await repository.GetById(id);
        return postTag == null ? NotFound() : postTag;
    }

    // POST: api/PostTag
    [HttpPost]
    public async Task<ActionResult<PostTag>> PostPostTag(PostTag postTag)
    {
        await repository.Insert(postTag);
        return CreatedAtAction(nameof(GetPostTag), new { id = postTag.ID }, postTag);
    }

    // PUT: api/PostTag/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutPostTag(int id, PostTag postTag)
    {
        if (id != postTag.ID) return BadRequest();
        return await repository.Update(postTag) ? NoContent() : NotFound();
    }

    // DELETE: api/PostTag/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostTag(int id)
    {
        return await repository.DeleteById(id) ? NoContent() : NotFound();
    }
}