using CMPSC487W_Project3.Entities;
using CMPSC487W_Project3.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CMPSC487W_Project3.Entities;

namespace CMPSC487W_Project3.Controllers
{
    public abstract class ControllerHelper : Controller
    {
        protected readonly AppDbContext _dbContext;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private static DbContextOptions<AppDbContext> _dbContextOptions;
        protected AppGetDataTools _AppGetDataTools;
        protected IConfiguration _configuration;
        protected Login CurrentAccount;

        public ControllerHelper(AppDbContext context, IConfiguration config, IWebHostEnvironment hostingEnvironment) : base()
        {
            _dbContext = context;
            string activeConnectionString = config.GetValue<string>("ConnectionStrings:ActiveDBString");
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(config.GetConnectionString(activeConnectionString)).Options;
            _configuration = config;
            _AppGetDataTools = new(context); //new(context, config);
            _hostingEnvironment = hostingEnvironment;
        }

        protected static AppGetDataTools GetTempDataTools()
        {
            return new AppGetDataTools(_dbContextOptions);
        }

        public static SelectList GenerateSelect<T>(IEnumerable<T> list,
            Func<T, string> textMapping, Func<T, string> valueMapping, bool firstDisabled = false
        )
        {
            SelectList dropDown = new(list.Select(i =>
                new SelectListItem(text: textMapping.Invoke(i), valueMapping.Invoke(i))), "Value", "Text");

            if (list.Any())
            {
                dropDown.First().Selected = true;
            }
            return dropDown;
        }

    }
}