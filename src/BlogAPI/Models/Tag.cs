using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace BlogAPI.Models
{
    [ExcludeFromCodeCoverage]
    public class Tag : IIdentified
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public required string Name { get; set; }

        [JsonIgnore]
        public ICollection<Post> Posts { get; } = [];

        public Tag(int ID, string Name, List<Post> Posts)
        {
            this.ID = ID;
            this.Name = Name;
            this.Posts = Posts;
        }

        public Tag(string Name, List<Post> Posts)
        {
            this.Name = Name;
            this.Posts = Posts;
        }

        public Tag() { }
    }
}