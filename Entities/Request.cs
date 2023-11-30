using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("Requests")]
    public class Request
    {
        [Column("RequestId")]
        public int Id { get; set; }
        [Column("RequestStatusId")]
        public int StatusId { get; set; }
        [Column("RequestTypeId")]
        public int TypeId { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("RequestTime")]
        public DateTime CreationDate { get; set; }
        [Column("PhotoPath")]
        public string? PhotoPath { get; set; }
        [Column("TenantId")]
        public int TenantId { get; set; }
    }
}
