namespace CMPSC487_Project2.Services
{
    public class AppGetDataTools
    {
        private readonly AppDbContext dbContext;

        public AppGetDataTools(AppDbContext context)
        {
            dbContext = context;
        }
    }
}
