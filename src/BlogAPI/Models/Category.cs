using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{

    public class Category
    {
        [Key]
        public int ID { get; set; }
        public required string Name { get; set; }

        public Category ( int ID, string Name )
        {
            this.ID = ID;
            this.Name = Name;
        }

        public Category ( string Name )
        {
            this.Name = Name;
        }

        public Category () {} 
    }
}