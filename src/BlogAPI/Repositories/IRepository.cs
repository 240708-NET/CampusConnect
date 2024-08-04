using BlogAPI.Models
{
    
    namespace BlogAPI.Repositories
    {
        public interface IRepository
        {
            Task<List<User>> GetAllUsers ();
            Task<User> GetUserByID ( int id );
            Task<User> AddUser ( User user );
            Task UpdateUser ( User user);
            Task DeleteEmployee ( User user );
            
        }
        
    }
}