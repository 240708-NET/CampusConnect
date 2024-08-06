using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{

    public class User
    {
        [Key]
        public int ID { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsAdmin { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }

        public User( int ID, string Username, string Password, List<Post> Posts, List<Comment> Comments, bool IsAdmin = false )
        {
            this.ID = ID;
            this.Username = Username;
            this.Password = Password;
            this.IsAdmin = IsAdmin;
            this.Posts = Posts;
            this.Comments = Comments;
        }

        public User( string Username, string Password, List<Post> Posts, List<Comment> Comments, bool IsAdmin = false)
        {
            this.Username = Username;
            this.Password = Password;
            this.IsAdmin = IsAdmin;
            this.Posts = Posts;
            this.Comments = Comments;
        }

        public User() {}
        
    }
}

