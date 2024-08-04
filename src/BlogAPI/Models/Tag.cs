using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{

    public class Tag
    {
        [Key]
        public int ID { get; set; }
        public required string Name { get; set; }

        public Tag ( int ID, string Name )
        {
            this.ID = ID;
            this.Name = Name;
        }

        public Tag ( string Name )
        {
            this.Name = Name;
        }

        public Tag () {} 
    }
}