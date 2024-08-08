namespace BlogAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Repositories;

[Route("api/[controller]")]
[ApiController]
public class TagController(ITagRepository repository) : ControllerBase
{
    // GET: api/Tag
    [HttpGet]
    public async Task<ActionResult<List<Tag>>> GetTags()
    {
        return await repository.Get();
    }

    // GET: api/Tag/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Tag>> GetTag(int id)
    {
        var tag = await repository.GetById(id);
        return tag is null ? NotFound() : tag;
    }

    // GET: api/Tag/{id}/Posts
    [HttpGet("{id}/Posts")]
    public async Task<ActionResult<List<Post>>> GetTagPosts(int id)
    {
        var posts = await repository.GetPostsByTagID(id);
        return posts is null ? NotFound() : posts;
    }

    // POST: api/Tag
    [HttpPost]
    public async Task<ActionResult<Tag>> PostTag(Tag tag)
    {
        await repository.Insert(tag);
        return CreatedAtAction(nameof(GetTag), new { id = tag.ID }, tag);
    }

    // PUT: api/Tag/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutTag(int id, Tag tag)
    {
        if (id != tag.ID) return BadRequest();
        return await repository.Update(tag) ? NoContent() : NotFound();
    }

    // DELETE: api/Tag/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        return await repository.DeleteById(id) ? NoContent() : NotFound();
    }
}
