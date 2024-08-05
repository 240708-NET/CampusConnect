namespace BlogAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;
using BlogAPI.Repositories;

[Route("api/[controller]")]
[ApiController]
public class UserController(BlogContext context) : ControllerBase
{
    private readonly IGenericRepository<User> _userRepository = new UserRepository(context);

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return await _userRepository.Get();
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null) return NotFound();
        return user;
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        await _userRepository.Insert(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.ID }, user);
    }

    // PUT: api/User/{id}
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> PutUser(int id, User user)
    {
        if (id != user.ID) return BadRequest();
        try
        {
            await _userRepository.Update(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            var targetUser = await _userRepository.GetById(id);
            if (targetUser == null) return NotFound();
            throw;
        }
        return NoContent();
    }

    // DELETE: api/User/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null) return NotFound();
        await _userRepository.Delete(user);
        return NoContent();
    }
}