using CMPSC487W_Project3.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Buffers;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CMPSC487W_Project3.Services
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


        public DbSet<Login> Logins { get; set; }
        public DbSet<LoginType> LoginTypes { get; set; }
        public DbSet<vwLogin> vwLogins { get; set; }
        public DbSet<vwRequest> VwRequests { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Appartment> Appartments { get; set; }
    }
}