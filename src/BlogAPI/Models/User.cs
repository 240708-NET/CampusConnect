using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{

    public class User
    {
        [Key]
        public int ID { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool Admin { get; set; }

        public User( int ID, string Username, string Password, bool Admin = false)
        {
            this.ID = ID;
            this.Username = Username;
            this.Password = Password;
            this.Admin = Admin;
        }

        public User( string Username, string Password, bool Admin = false)
        {
            this.Username = Username;
            this.Password = Password;
            this.Admin = Admin;
        }

        public User() {}
        
    }
}