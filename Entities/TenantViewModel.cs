namespace CMPSC487W_Project3.Entities
{
    public class TenantViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AppartmentNumber { get; set; }
        public string PhoneNumber { get; set; }

        public List<vwRequest> Requests { get; set; }

    }
}
