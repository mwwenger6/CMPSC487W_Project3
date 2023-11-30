using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("vwLogin")]
    public class vwLogin
    {
        [Column("LoginId")]
        public int Id { get; set; }
        [Column("LoginTypeId")]
        public int TypeId { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("LoginTypeName")]
        public string TypeName { get; set; }
        [Column("AppartmentNumber")]
        public string AppartmentNumber { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("PhoneNumber")]
        public string Phone { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("CheckInDate")]
        public DateTime CheckIn { get; set; }
        [Column("CheckOutDate")]
        public DateTime? CheckOut { get; set; }
        [Column("TenantId")]
        public int TenantId { get; set; }
    }
}
