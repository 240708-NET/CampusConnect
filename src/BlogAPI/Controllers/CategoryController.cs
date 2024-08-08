namespace BlogAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Repositories;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryRepository repository) : ControllerBase
{
    // GET: api/Category
    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories()
    {
        return await repository.Get();
    }

    // GET: api/Category/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await repository.GetById(id);
        return category is null ? NotFound() : category;
    }

    // GET: api/Category/{id}/Posts
    [HttpGet("{id}/Posts")]
    public async Task<ActionResult<List<Post>>> GetCategoryPosts(int id)
    {
        var posts = await repository.GetPostsByCategoryID(id);
        return posts is null ? NotFound() : posts;
    }

    // POST: api/Category
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        await repository.Insert(category);
        return CreatedAtAction(nameof(GetCategory), new { id = category.ID }, category);
    }

    // PUT: api/Category/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.ID) return BadRequest();
        return await repository.Update(category) ? NoContent() : NotFound();
    }

    // DELETE: api/Category/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        return await repository.DeleteById(id) ? NoContent() : NotFound();
    }
}
