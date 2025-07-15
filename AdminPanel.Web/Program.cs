using AdminPanel.BLL.Managers;
using AdminPanel.BLL.Service;
using AdminPanel.DAL.Context;
using AdminPanel.DAL.Interfaces;
using AdminPanel.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DI: IUserService - UserManager kaydý
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// MVC Controller + View desteði
builder.Services.AddControllersWithViews();

// Cookie Authentication yapýlandýrmasý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";          // Giriþ sayfasý URL'si
        options.AccessDeniedPath = "/Account/AccessDenied";  // Yetkisiz sayfa (isteðe baðlý)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Cookie süresi
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseHsts(); // Ýstersen güvenlik için ekleyebilirsin
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Kimlik doðrulama orta katmaný
app.UseAuthorization();   // Yetkilendirme orta katmaný

// Default route (controller/action/id)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
