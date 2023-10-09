using Microsoft.EntityFrameworkCore;
using CMPSC487_Project2.Services;


var builder = WebApplication.CreateBuilder(args);

string activeConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:ActiveDBString");
string connectionString = builder.Configuration.GetConnectionString(activeConnectionString);
//builder.Services.add;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

ConfigurationManager config = builder.Configuration;
builder.Services.AddControllers();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});
app.Run();
