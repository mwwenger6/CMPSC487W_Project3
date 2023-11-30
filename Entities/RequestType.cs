using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("RequestTypes")]
    public class RequestType
    {
        [NotMapped] public const string KITCHEN = "Kitchen";
        [NotMapped] public const string BATHROOM = "Bathroom";
        [NotMapped] public const string BEDROOM = "Bedroom";
        [NotMapped] public const string OTHER = "Other";

        [Column("RequestTypeId")]
        public int Id { get; set; }
        [Column("RequestTypeName")]
        public string Name { get; set; }
    }
}
