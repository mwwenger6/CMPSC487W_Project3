using Microsoft.EntityFrameworkCore;
using CMPSC487W_Project2.Services;
using CMPSC487W_Project2.Entities;

namespace CMPSC487W_Project2.Services
{
    public class AppGetDataTools
    {
        private readonly AppDbContext dbContext;

        public AppGetDataTools(AppDbContext context)
        {
            dbContext = context;
        }

        public IEnumerable<Item> GetItems()
        {
            return dbContext.Items;
        }

        public Item GetItem(int id)
        {
            return dbContext.Items
                .Where(i => i.Id == id).FirstOrDefault();
        }

        public void AddItem(Item item)
        {
            if (dbContext.Items.Any(i => i.Id == item.Id))
            {
                dbContext.Items
                    .Where(a => a.Id == item.Id)
                    .ExecuteUpdate(i => i
                        .SetProperty(a => a.Description, item.Description)
                        .SetProperty(a => a.Name, item.Name)
                        .SetProperty(a => a.FilePath, item.FilePath)
                    );
            }
            else
            {
                dbContext.Items.Add(item);
                dbContext.SaveChanges();
            }
        }

        public void DeleteItem(int id)
        {
            if(id > 0)
            {
                dbContext.Items
                    .Where(i => i.Id == id)
                    .ExecuteDelete();
            }
        }
    }
}
