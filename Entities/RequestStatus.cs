using System.ComponentModel.DataAnnotations.Schema;

namespace CMPSC487W_Project3.Entities
{
    [Table("RequestStatuses")]
    public class RequestStatus
    {
        [NotMapped] public const string PENDING = "Pending";
        [NotMapped] public const string COMPLETED = "Completed";
        [Column("RequestStatusId")]
        public int Id { get; set; }
        [Column("RequestStatusName")]
        public string Name { get; set; }
    }
}
