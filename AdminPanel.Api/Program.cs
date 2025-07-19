using AdminPanel.BLL.Managers;
using AdminPanel.BLL.Service;
using AdminPanel.DAL.Context;
using AdminPanel.DAL.Interfaces;
using AdminPanel.DAL.Repositories;
using AdminPanel.Api.Validators; // FluentValidation validator'lar�n�n bulundu�u namespace
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Swagger i�in gerekli
using Microsoft.AspNetCore.Authentication.Cookies; // E�er MVC i�in Cookie tabanl� kimlik do�rulama kullan�yorsan

var builder = WebApplication.CreateBuilder(args);

// Yap�land�rma dosyalar�n� y�kle (mevcut ayarlar�n� korur)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// DbContext Kayd�
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection: �� mant��� (BLL) ve Veri Eri�im Katman� (DAL) servisleri
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// FluentValidation Servisleri: Validat�rleri otomatik bulup kaydeder
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>(); // Kendi validator'lar�n�n oldu�u bir validator s�n�f� belirtmelisin
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>(); // Birden fazla validator s�n�f�n varsa ekleyebilirsin
builder.Services.AddFluentValidationAutoValidation(); // Otomatik do�rulama tetiklenmesini sa�lar

// *** �NEML�: HEM API CONTROLLER'LARI HEM DE MVC CONTROLLER'LARI ���N BURASI! ***
// Hem View'lar� i�leyebilen MVC Controller'lar hem de JSON/XML d�nd�ren API Controller'lar i�in.
builder.Services.AddControllersWithViews();

// Cookie Authentication yap�land�rmas� (E�er MVC k�sm�nda kullan�c� giri�i ve yetkilendirme kullan�yorsan)
// API i�in farkl� bir kimlik do�rulama y�ntemi (�rn. JWT) kullan�l�yorsa, o ayr�ca yap�land�r�lmal� veya bu k�s�m ��kar�labilir.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";      // Kullan�c� giri� sayfas�
        options.AccessDeniedPath = "/Account/AccessDenied";  // Yetkisiz eri�im sayfas�
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Cookie �mr�
    });

// Swagger/OpenAPI Servisleri (API d�k�mantasyonu i�in)
builder.Services.AddEndpointsApiExplorer(); // Swagger UI i�in endpointleri ke�fetmeye yarar
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AdminPanel API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header kullan�m�: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id= "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   
    app.UseSwaggerUI();   
}
else
{
    app.UseExceptionHandler("/Home/Error"); 
}

app.UseHttpsRedirection(); 
app.UseStaticFiles();     

app.UseRouting(); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.Run();