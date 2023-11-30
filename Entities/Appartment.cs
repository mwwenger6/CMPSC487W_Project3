using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("AvailableAppartments")]
    public class Appartment
    {
        [Column("AppartmentNumber")]
        public string Id { get; set; }
        [Column("IsAvailable")]
        public bool Available { get; set; }
    }
}
