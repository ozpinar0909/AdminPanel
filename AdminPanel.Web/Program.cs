using AdminPanel.DAL.Context;
using AdminPanel.DAL.Interfaces;
using AdminPanel.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Connection string'i al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i DI container'a ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Generic Repository'yi DI container'a ekle
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Controller ve View servisi
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Uygulama pipeline'ý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Varsayýlan route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
