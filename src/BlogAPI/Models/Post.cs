using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    
    public class Post
    {
        [Key]
        public int ID { get; set; }
        public required string Topic { get; set; }
        public required string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public Category PostCategory { get; set; }
        public User Poster { get; set; }
        public List<Tag> Tags { get; set; }

        public Post ( int ID, string Topic, string Body, DateTime CreatedAt, DateTime EditedAt, Category Category, User Poster, List<Tag> Tags )
        {
            this.ID = ID;
            this.Topic = Topic;
            this.Body = Body;
            this.CreatedAt = CreatedAt;
            this.EditedAt = EditedAt;
            this.PostCategory = Category;
            this.Poster = Poster;
            this.Tags = Tags;
        }

        public Post ( string Topic, string Body, Category Category, User Poster, List<Tag> Tags )
        {
            this.Topic = Topic;
            this.Body = Body;
            this.CreatedAt = DateTime.Now;
            this.EditedAt = DateTime.Now;
            this.PostCategory = Category;
            this.Poster = Poster;
            this.Tags = Tags;
        }

        public Post () {}
    }
}