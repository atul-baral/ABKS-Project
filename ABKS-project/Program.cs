using ABKS_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using ABKS_project.Areas.Ecommerce.Models;
using ABKS_project.Repositories;
using ABKS_project.Utilities;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddControllersWithViews();

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();

builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddDbContext<abksContext>(item => item.UseSqlServer(config.GetConnectionString("abks_db")));
builder.Services.AddDbContext<productContext>(item => item.UseSqlServer(config.GetConnectionString("abks_db")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<EmailService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });

    options.AddPolicy("UserOnly", policy =>
    {
        policy.RequireRole("User");
    });
});



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
