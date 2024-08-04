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

        public Comment ( int ID, string Body, DateTime CreatedAt, DateTime EditedAt)
        {
            this.ID = ID;
            this.Body = Body;
            this.CreatedAt = CreatedAt;
            this.EditedAt = EditedAt;
        }

        public Comment ( string Body )
        {
            this.Body = Body;
            // should CreatedAt and EditedAt be just datetime now when created
        }

        public Comment () {}
    }
}