using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("Logins")]
    public class Login
    {
        [Column("LoginId")]
        public int Id { get; set; }
        [Column("LoginTypeId")]
        public int TypeId { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Password")]
        public string Password { get; set; }
    }
}
