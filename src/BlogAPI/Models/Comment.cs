using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        public required string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public Post OriginalPost { get; set; }
        public User Commenter { get; set; }
        public Comment ParentComment { get; set; }
        public List<Comment> ChildComments { get; set; }


        public Comment ( int ID, string Body, DateTime CreatedAt, DateTime EditedAt, Post OriginalPost, User Commenter, Comment ParentComment, List<Comment> ChildComments )
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

        public Comment ( string Body, Post OriginalPost, User Commenter, Comment ParentComment )
        {
            this.Body = Body;
            this.CreatedAt = DateTime.Now;
            this.EditedAt = DateTime.Now;
            this.OriginalPost = OriginalPost;
            this.Commenter = Commenter;
            this.ParentComment = ParentComment;
            this.ChildComments = new List<Comment>();
        }

        public Comment () {}
    }
}