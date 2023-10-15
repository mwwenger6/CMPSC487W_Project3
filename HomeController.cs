using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CMPSC487W_Project2.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using CMPSC487W_Project2.Entities;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;



namespace CMPSC487W_Project2.Controllers
{
    public class HomeController : ControllerHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(AppDbContext context, IConfiguration config, IWebHostEnvironment hostingEnvironment) 
            : base(context, config, hostingEnvironment){}


        [HttpGet]
        [Route("[controller]/[action]")]
        public IActionResult Store()
        {
            List<Item> items = _AppGetDataTools.GetItems().ToList();
            return View(items);
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public IActionResult _AddItem(int id)
        {
            ItemViewModel itemViewModel = new()
            {
                FilePath = "tempPath",
                FormFile = new FormFile(new MemoryStream(), 0, 0, null, null)
            };
            if(id > 0)
            {
                Item item = _AppGetDataTools.GetItem(id);
                itemViewModel.Description = item.Description;
                itemViewModel.Name = item.Name;
                itemViewModel.FilePath = item.FilePath;
                itemViewModel.Id = id;
            }
            return PartialView("_AddItemModal", itemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ItemViewModel item)
        {
            if (ModelState.IsValid)
            {
                var basePath = Directory.GetCurrentDirectory();
                var uploadDir = Path.Combine(basePath, "wwwroot", "Imgs");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var fileName = Path.GetFileNameWithoutExtension(item.FormFile.FileName);
                var extension = Path.GetExtension(item.FormFile.FileName);

                var filePath = Path.Combine(uploadDir, fileName + extension);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await item.FormFile.CopyToAsync(fileStream);
                }

                var relativePath = $"{fileName + extension}";
                item.FilePath = relativePath;

                try
                {
                    Item addingItem = new()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name,
                        FilePath = item.FilePath
                    };
                    _AppGetDataTools.AddItem(addingItem);
                }
                catch (Exception ex)
                {
                    return Json("failure");
                }
                return RedirectToAction("Store");
            }
            if (item.Id != 0)
            {
                try
                {
                    Item addingItem = new()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name,
                        FilePath = item.FilePath
                    };
                    _AppGetDataTools.AddItem(addingItem);
                }
                catch (Exception ex)
                {
                    return Json("failure");
                }
                return RedirectToAction("Store");
            }
            return Json("failure");
        }

        [HttpPost]
        public IActionResult DeleteItem(int id)
        {
            try
            {
                _AppGetDataTools.DeleteItem(id);
            }
            catch(Exception ex)
            {
                return Json("failure");
            }
            return RedirectToAction("Store");
        }
    }
}
