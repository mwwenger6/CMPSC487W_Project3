using Azure.Core;
using CMPSC487W_Project3.Entities;
using CMPSC487W_Project3.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Request = CMPSC487W_Project3.Entities.Request;

namespace CMPSC487W_Project3.Controllers
{
    public class HomeController : ControllerHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(AppDbContext context, IConfiguration config, IWebHostEnvironment hostingEnvironment)
            : base(context, config, hostingEnvironment) { }


        [HttpGet]
        [Route("Home/_Login")]
        public IActionResult _Login()
        {
            LoginViewModel vm = new LoginViewModel();
            return View(vm);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public IActionResult Login(LoginViewModel model)
        {
            Login login = _AppGetDataTools.IsLogin(model.Username, model.Password);
            if (login != null)
            {
                HttpContext.Session.SetInt32("UserId", login.Id);
                if (login.TypeId == 1)
                {
                    return RedirectToAction("Tenant");
                }
                else if(login.TypeId == 3)
                {
                    return RedirectToAction("Maintenance");
                }
                else
                {
                    return RedirectToAction("Manager");
                }
            }
            return RedirectToAction("_Login");

        }

        [HttpGet]
        public IActionResult Manager()
        {
            List<vwLogin> vwLogins = _AppGetDataTools.GetAllTenants().ToList();
            return View(vwLogins);
        }

        [HttpGet]
        public IActionResult Maintenance()
        {
            List<vwRequest> requests = _AppGetDataTools.GetVwRequests().ToList();
            return View(requests);
        }

        [HttpGet]
        public IActionResult Tenant()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            Tenant tenant = _AppGetDataTools.GetTenantFromLogin(userId ?? 0);
            List<vwRequest> requests = _AppGetDataTools.GetTenantRequests(tenant.Id).ToList();
            TenantViewModel vm = new()
            {
                Id = tenant.Id,
                Requests = requests
            };
            return View(vm);
        }

        [HttpGet]
        [Route("Home/_AddTenant/{tenantId}")]
        public IActionResult _AddTenant(int tenantId)
        {
            if(tenantId > 0)
            {
                vwLogin vm = _AppGetDataTools.GetTenantLogin(tenantId);
                return PartialView("_AddTenant", vm);
            }
            else
            {
                vwLogin vm = new vwLogin();
                return PartialView("_AddTenant", vm);
            }

        }

        [HttpPost]
        [Route("Home/DeleteTenant/{tenantId}")]
        public IActionResult DeleteTenant(int tenantId)
        {
            int loginId = _AppGetDataTools.GetTenantLogin(tenantId).Id;
            _AppGetDataTools.DeleteTenant(tenantId, loginId);
            return RedirectToAction("Manager");
        }

        [HttpPost]
        public IActionResult AddTenant(vwLogin loginTenant)
        {
            Tenant tenant = new()
            {
                Id = loginTenant.TenantId,
                Email = loginTenant.Email,
                Phone = loginTenant.Phone,
                Name = loginTenant.Name,
                CheckIn = DateTime.Now,
                CheckOut = loginTenant.CheckOut,
                AppartmentNumber = loginTenant.AppartmentNumber,
                LoginId = loginTenant.Id
            };
            Login login = new()
            {
                Id = loginTenant.Id,
                Username = loginTenant.Username,
                Password = loginTenant.Password,
                TypeId = 1,
            };
            int loginId = _AppGetDataTools.AddLogin(login);
            tenant.LoginId = loginId;
            if(tenant.Id > 0)
            {
                string previousAppartment = _AppGetDataTools.GetTenantLogin(tenant.Id).AppartmentNumber;
                _AppGetDataTools.UpdateAppartments(previousAppartment, false);
            }
            _AppGetDataTools.AddTenant(tenant);
            _AppGetDataTools.UpdateAppartments(tenant.AppartmentNumber, true);
            return RedirectToAction("Manager");
        }

        [HttpGet]
        [Route("Home/_AddRequest/{tenantId}")]
        public IActionResult _AddRequest(int tenantId)
        {
            RequestViewModel vm = new()
            {
                TenantId = tenantId,
                FilePath = "tempPath",
                FormFile = new FormFile(new MemoryStream(), 0, 0, null, null)
            };
            return PartialView("_AddRequestModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRequest(RequestViewModel request)
        {
            if (ModelState.IsValid)
            {
                if(request.FormFile != null)
                {
                    var basePath = Directory.GetCurrentDirectory();
                    var uploadDir = Path.Combine(basePath, "wwwroot", "Imgs");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var fileName = Path.GetFileNameWithoutExtension(request.FormFile.FileName);
                    var extension = Path.GetExtension(request.FormFile.FileName);

                    var filePath = Path.Combine(uploadDir, fileName + extension);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.FormFile.CopyToAsync(fileStream);
                    }

                    var relativePath = $"{fileName + extension}";
                    request.FilePath = relativePath;
                }
                else
                    request.FilePath = "";
                try
                {
                    Request newRequest = new()
                    {
                        Id = request.Id,
                        Description = request.Description,
                        TypeId = request.TypeId,
                        PhotoPath = request.FilePath,
                        StatusId = 1,
                        TenantId = request.TenantId,
                        CreationDate = DateTime.Now,
                    };
                    _AppGetDataTools.AddRequest(newRequest);
                }
                catch (Exception ex)
                {
                    return Json("failure");
                }
                return RedirectToAction("Tenant");
            }
            return Json("failure");
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public IActionResult UpdateRequest(int id)
        {
            try
            {
                Request newRequest = new()
                {
                    Id = id,
                    StatusId = 2,
                };
                _AppGetDataTools.AddRequest(newRequest);
            }
            catch (Exception ex)
            {
                return Json("failure");
            }
            return RedirectToAction("Maintenance");
        }

        //[HttpPost]
        //public IActionResult DeleteItem(int id)
        //{
        //    try
        //    {
        //        _AppGetDataTools.DeleteItem(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("failure");
        //    }
        //    return RedirectToAction("Store");
        //}


        public static SelectList RequestTypes()
        {
            AppGetDataTools tempTools = GetTempDataTools();
            return GenerateSelect(tempTools.GetRequestTypes(), m => m.Name, m => m.Id.ToString());
        }
        public static SelectList AvailableNumbers(string appartment)
        {
            AppGetDataTools tempTools = GetTempDataTools();
            return GenerateSelect(tempTools.GetAvailableAppartments(appartment), m => m.Id, m => m.Id);
        }
    }
}
