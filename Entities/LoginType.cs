using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("LoginTypes")]
    public class LoginType
    {

        [NotMapped] public const string TENANT = "Tenant";
        [NotMapped] public const string MANAGER = "Manager";
        [NotMapped] public const string MAINTENance = "Maintenance";
        [Column("LoginTypeId")]
        public int Id { get; set; }
        [Column("LoginTypeName")]
        public string Name { get; set; }
    }
}
