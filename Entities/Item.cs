using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project2.Entities
{
    [Table("Items")]
    public class Item
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string? Name { get; set; }
        [Column("Description")]
        public string? Description { get; set; }
        [Column("FilePath")]
        public string? FilePath { get; set; }
    }
}
