using CMPSC487W_Project3.Entities;
using CMPSC487W_Project3.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CMPSC487W_Project3.Services
{
    public class AppGetDataTools
    {
        private readonly AppDbContext dbContext;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public AppGetDataTools(AppDbContext context)
        {
            dbContext = context;
        }
        public AppGetDataTools(DbContextOptions<AppDbContext> contextOptions)
        {
            dbContext = new(contextOptions);
        }


        public IEnumerable<RequestType> GetRequestTypes()
        {
            return dbContext.RequestTypes;
        }

        public Login IsLogin(string username, string password)
        {
            return dbContext.Logins.Where(l => l.Username == username && l.Password == password).FirstOrDefault();
        }

        public Tenant GetTenantFromLogin(int loginId)
        {
            return dbContext.Tenants.Where(t => t.LoginId == loginId).FirstOrDefault();
        }

        public vwLogin GetTenantLogin(int tenantId)
        {
            return dbContext.vwLogins.Where(t => t.TenantId == tenantId).FirstOrDefault();
        }
        public IEnumerable<vwRequest> GetVwRequests()
        {
            return dbContext.VwRequests;
        }
        public IEnumerable<vwRequest> GetTenantRequests(int id)
        {
            return dbContext.VwRequests
                .Where(r => r.TenantId == id);
        }
        public IEnumerable<vwLogin> GetAllTenants()
        {
            return dbContext.vwLogins;
        }
        public IEnumerable<Appartment> GetAvailableAppartments(string appartment)
        {
            return dbContext.Appartments
                .Where(a => !a.Available || a.Id == appartment);
        }

        public void AddRequest(Request request)
        {
            if (dbContext.Requests.Any(i => i.Id == request.Id))
            {
                dbContext.Requests
                    .Where(a => a.Id == request.Id)
                    .ExecuteUpdate(i => i
                        .SetProperty(a => a.StatusId, 2)
                    );
            }
            else
            {
                dbContext.Requests.Add(request);
                dbContext.SaveChanges();
            }
        }

        public void AddTenant(Tenant tenant)
        {
            if (dbContext.Tenants.Any(i => i.Id == tenant.Id))
            {
                dbContext.Tenants
                    .Where(a => a.Id == tenant.Id)
                    .ExecuteUpdate(i => i
                        .SetProperty(t => t.Name, tenant.Name)
                        .SetProperty(t => t.AppartmentNumber, tenant.AppartmentNumber)
                        .SetProperty(t => t.CheckIn, tenant.CheckIn)
                        .SetProperty(t => t.CheckOut, tenant.CheckOut)
                        .SetProperty(t => t.Phone, tenant.Phone)
                        .SetProperty(t => t.Email, tenant.Email)
                    );
            }
            else
            {
                dbContext.Tenants.Add(tenant);
                dbContext.SaveChanges();
            }
        }
        public int AddLogin(Login login)
        {
            if (dbContext.Logins.Any(i => i.Id == login.Id))
            {
                dbContext.Logins
                    .Where(a => a.Id == login.Id)
                    .ExecuteUpdate(i => i
                        .SetProperty(t => t.Username, login.Username)
                        .SetProperty(t => t.Password, login.Password)
                    );
                return login.Id;
            }
            else
            {
                dbContext.Logins.Add(login);
                dbContext.SaveChanges();
                return login.Id;
            }
        }

        public void UpdateAppartments(string id, bool occupied)
        {
            dbContext.Appartments
                    .Where(a => a.Id == id)
                    .ExecuteUpdate(i => i
                        .SetProperty(t => t.Available, occupied)
                    );
        }
        public void DeleteTenant(int tenantId, int loginId)
        {
            if (tenantId > 0)
            {
                dbContext.Tenants
                    .Where(i => i.Id == tenantId)
                    .ExecuteDelete();
            }
            if (loginId > 0)
            {
                dbContext.Logins
                    .Where(i => i.Id == loginId)
                    .ExecuteDelete();
            }
        }
    }
}
