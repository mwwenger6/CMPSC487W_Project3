using Microsoft.EntityFrameworkCore;
using CMPSC487W_Project3.Services;
using System;


var builder = WebApplication.CreateBuilder(args);

string activeConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:ActiveDBString");
string connectionString = builder.Configuration.GetConnectionString(activeConnectionString);
//builder.Services.add;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDistributedMemoryCache();

builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // You can change the timeout duration
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

ConfigurationManager config = builder.Configuration;

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.MapRazorPages();

app.Run();