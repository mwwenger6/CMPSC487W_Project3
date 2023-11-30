using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("Tenant")]
    public class Tenant
    {
        [Column("TenantId")]
        public int Id { get; set; }
        [Column("LoginId")]
        public int LoginId { get; set; }
        [Column("AppartmentNumber")]
        public string AppartmentNumber { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("PhoneNumber")]
        public string Phone {  get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("CheckInDate")]
        public DateTime CheckIn { get; set; }
        [Column("CheckOutDate")]
        public DateTime? CheckOut { get; set; }
    }
}
