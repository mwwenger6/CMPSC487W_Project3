using System.ComponentModel.DataAnnotations;

namespace CMPSC487W_Project2.Entities
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        public string FilePath { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
