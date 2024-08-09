namespace BlogAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Repositories;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserRepository repository) : ControllerBase
{
    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return await repository.Get();
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await repository.GetById(id);
        return user is null ? NotFound() : user;
    }

    // GET: api/User/{id}/Posts
    [HttpGet("{id}/Posts")]
    public async Task<ActionResult<List<Post>>> GetUserPosts(int id)
    {
        var posts = await repository.GetPostsByUserID(id);
        return posts is null ? NotFound() : posts;
    }

    // GET: api/User/{id}/Comments
    [HttpGet("{id}/Comments")]
    public async Task<ActionResult<List<Comment>>> GetUserComments(int id)
    {
        var comments = await repository.GetCommentsByUserID(id);
        return comments is null ? NotFound() : comments;
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        await repository.Insert(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.ID }, user);
    }

    // PUT: api/User/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutUser(int id, User user)
    {
        if (id != user.ID) return BadRequest();
        return await repository.Update(user) ? NoContent() : NotFound();
    }

    // DELETE: api/User/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        return await repository.DeleteById(id) ? NoContent() : NotFound();
    }
}
