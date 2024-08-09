using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace BlogAPI.Models
{
    [ExcludeFromCodeCoverage]
    public class Comment : IIdentified
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int OriginalPostID { get; set; }
        public int? ParentCommentID { get; set; }
        [Required]
        public int CommenterID { get; set; }
        [Required]
        public required string Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public Post OriginalPost { get; set; } = null!;
        [JsonIgnore]
        public Comment? ParentComment { get; set; }
        [JsonIgnore]
        public User Commenter { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Comment> ChildComments { get; } = [];


        public Comment(int ID, string Body, DateTime CreatedAt, DateTime EditedAt, Post OriginalPost, User Commenter, Comment ParentComment, List<Comment> ChildComments)
        {
            this.ID = ID;
            this.Body = Body;
            this.CreatedAt = CreatedAt;
            this.EditedAt = EditedAt;
            this.OriginalPost = OriginalPost;
            this.Commenter = Commenter;
            this.ParentComment = ParentComment;
            this.ChildComments = ChildComments;
        }

        public Comment(string Body, Post OriginalPost, User Commenter, Comment ParentComment)
        {
            this.Body = Body;
            this.CreatedAt = DateTime.Now;
            this.EditedAt = DateTime.Now;
            this.OriginalPost = OriginalPost;
            this.Commenter = Commenter;
            this.ParentComment = ParentComment;
            this.ChildComments = new List<Comment>();
        }

        public Comment() { }
    }
}