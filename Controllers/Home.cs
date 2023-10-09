using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CMPSC487_Project2.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using CMPSC487_Project2.Entities;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;

namespace CMPSC487_Project2.Controllers
{
    public abstract class ControllerHelper : Controller
    {
        protected readonly AppDbContext _dbContext;
        protected DbContextOptions<AppDbContext> _dbContextOptions;
        protected AppGetDataTools _AppGetDataTools;
        protected IConfiguration _configuration;
        public ControllerHelper(AppDbContext context, IConfiguration config) : base()
        {
            _dbContext = context;
            string activeConnectionString = config.GetValue<string>("ConnectionStrings:ActiveDBString");
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(config.GetConnectionString(activeConnectionString)).Options;
            _configuration = config;
            _AppGetDataTools = new(context); //new(context, config);
        }
    }
}