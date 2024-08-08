using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogAPI.Models
{

    public class Post : IIdentified
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int PosterID { get; set; }
        [Required]
        public required string Topic { get; set; }
        [Required]
        public required string Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public Category Category { get; set; } = null!;
        [JsonIgnore]
        public User Poster { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Tag> Tags { get; } = [];

        public Post(int ID, string Topic, string Body, DateTime CreatedAt, DateTime EditedAt, Category Category, User Poster, List<Tag> Tags)
        {
            this.ID = ID;
            this.Topic = Topic;
            this.Body = Body;
            this.CreatedAt = CreatedAt;
            this.EditedAt = EditedAt;
            this.Category = Category;
            this.Poster = Poster;
            this.Tags = Tags;
        }

        public Post(string Topic, string Body, Category Category, User Poster, List<Tag> Tags)
        {
            this.Topic = Topic;
            this.Body = Body;
            this.CreatedAt = DateTime.Now;
            this.EditedAt = DateTime.Now;
            this.Category = Category;
            this.Poster = Poster;
            this.Tags = Tags;
        }

        public Post() { }
    }
}