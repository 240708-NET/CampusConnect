using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{

    public class Tag
    {
        [Key]
        public int ID { get; set; }
        public required string Name { get; set; }
        public List<Post> Posts { get; set; }

        public Tag ( int ID, string Name, List<Post> Posts )
        {
            this.ID = ID;
            this.Name = Name;
            this.Posts = Posts;
        }

        public Tag ( string Name, List<Post> Posts )
        {
            this.Name = Name;
            this.Posts = Posts;
        }

        public Tag () {} 
    }
}