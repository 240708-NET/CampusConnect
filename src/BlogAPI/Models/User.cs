using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogAPI.Models
{

    public class User : IIdentified
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }

        [JsonIgnore]
        public ICollection<Post> Posts { get; } = [];
        [JsonIgnore]
        public ICollection<Comment> Comments { get; } = [];

        public User(int ID, string Username, string Password, List<Post> Posts, List<Comment> Comments, bool IsAdmin = false)
        {
            this.ID = ID;
            this.Username = Username;
            this.Password = Password;
            this.IsAdmin = IsAdmin;
            this.Posts = Posts;
            this.Comments = Comments;
        }

        public User(string Username, string Password, List<Post> Posts, List<Comment> Comments, bool IsAdmin = false)
        {
            this.Username = Username;
            this.Password = Password;
            this.IsAdmin = IsAdmin;
            this.Posts = Posts;
            this.Comments = Comments;
        }

        public User() { }

    }
}
