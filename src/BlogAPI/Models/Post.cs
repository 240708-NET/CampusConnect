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

        public Post ( int ID, string Topic, string Body, DateTime CreatedAt, DateTime EditedAt)
        {
            this.ID = ID;
            this.Topic = Topic;
            this.Body = Body;
            this.CreatedAt = CreatedAt;
            this.EditedAt = EditedAt;
        }

        public Post ( string Topic, string Body )
        {
            this.Topic = Topic;
            this.Body = Body;
            // should CreatedAt and EditedAt be just datetime now when created
        }

        public Post () {}
    }
}