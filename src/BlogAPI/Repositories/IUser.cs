using BlogAPI.Models;

    
namespace BlogAPI.Repositories
{
    public interface IUser
    {
        public Task<List<User>> GetAllUsers ();
        public Task<User>? GetUserByID ( int id );
        public Task<User> AddUser ( User user );
        public Task UpdateUser ( User user);
        public Task DeleteUser ( User user );
        
    }
    
}
 