using System.ComponentModel.DataAnnotations;

namespace CMPSC487W_Project3.Entities
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        public string FilePath { get; set; }
        public IFormFile? FormFile { get; set; }
        public int TenantId { get; set; }
    }
}
