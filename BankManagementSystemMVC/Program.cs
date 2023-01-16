using BankManagementSystemApi.Implementations;
using BankManagementSystemApi.Services;
using BankManagementSystemIdentity.Models;
using BankManagementSystemIdentity.Repository;
using BankManagementSystemMVC.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
var smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");

builder.Services.AddFluentEmail("Noel.Girma@outlook.com").AddRazorRenderer().AddSmtpSender("localhost", 587);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");


//For Identity
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(connectionString));

//For Main Api
builder.Services.AddDbContext<BankManagementSystemApi.Repository.AppDbContext>(options =>
options.UseSqlServer(connectionString));

//builder.Services.AddScoped<ITransaction, TransactionImplementation>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.Stores.MaxLengthForKeys = 128)
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<ManagersController>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
