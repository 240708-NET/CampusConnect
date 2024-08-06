using BlogApi.Models;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace BlogAPI.Repositories
{
    public class UserRepository : IUser {

        private readonly BlogContext _context;

        public UserRepository(BlogContext blogContext)
        {
            _context = blogContext;
        }

        public async Task<List<User>> GetAllUsers ()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User>? GetUserByID ( int id )
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> AddUser ( User user )
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task UpdateUser ( User user )
        {
            User existingUser = _context.Users.Find(user.ID);
            
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.IsAdmin = user.IsAdmin;

            await _context.SaveChangesAsync();

        }
        public async Task DeleteUser ( User user )
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


    }   
}