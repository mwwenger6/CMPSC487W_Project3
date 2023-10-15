using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CMPSC487W_Project2.Entities;
using static System.Net.Mime.MediaTypeNames;
using System.Buffers;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;
using System.Threading.Tasks;

namespace CMPSC487W_Project2.Services
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


        public DbSet<Item> Items { get; set; }
        
    }
}