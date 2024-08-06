using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{

    public class Category
    {
        [Key]
        public int ID { get; set; }
        public required string Name { get; set; }
        public List<Post> Posts { get; set; }

        public Category ( int ID, string Name, List<Post> Posts )
        {
            this.ID = ID;
            this.Name = Name;
            this.Posts = Posts;
        }

        public Category ( string Name, List<Post> Posts )
        {
            this.Name = Name;
            this.Posts = Posts;
        }

        public Category () {} 
    }
}