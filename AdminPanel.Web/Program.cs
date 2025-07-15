using AdminPanel.BLL.Managers;
using AdminPanel.BLL.Service;
using AdminPanel.DAL.Context;
using AdminPanel.DAL.Interfaces;
using AdminPanel.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DI: IUserService - UserManager kayd�
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// MVC Controller + View deste�i
builder.Services.AddControllersWithViews();

// Cookie Authentication yap�land�rmas�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";          // Giri� sayfas� URL'si
        options.AccessDeniedPath = "/Account/AccessDenied";  // Yetkisiz sayfa (iste�e ba�l�)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Cookie s�resi
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseHsts(); // �stersen g�venlik i�in ekleyebilirsin
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Kimlik do�rulama orta katman�
app.UseAuthorization();   // Yetkilendirme orta katman�

// Default route (controller/action/id)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
