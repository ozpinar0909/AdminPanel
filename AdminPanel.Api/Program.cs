using AdminPanel.BLL.Managers;
using AdminPanel.BLL.Service;
using AdminPanel.DAL.Context;
using AdminPanel.DAL.Interfaces;
using AdminPanel.DAL.Repositories;
using AdminPanel.Api.Validators; // FluentValidation validator'larýnýn bulunduðu namespace
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Swagger için gerekli
using Microsoft.AspNetCore.Authentication.Cookies; // Eðer MVC için Cookie tabanlý kimlik doðrulama kullanýyorsan

var builder = WebApplication.CreateBuilder(args);

// Yapýlandýrma dosyalarýný yükle (mevcut ayarlarýný korur)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// DbContext Kaydý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection: Ýþ mantýðý (BLL) ve Veri Eriþim Katmaný (DAL) servisleri
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// FluentValidation Servisleri: Validatörleri otomatik bulup kaydeder
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>(); // Kendi validator'larýnýn olduðu bir validator sýnýfý belirtmelisin
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>(); // Birden fazla validator sýnýfýn varsa ekleyebilirsin
builder.Services.AddFluentValidationAutoValidation(); // Otomatik doðrulama tetiklenmesini saðlar

// *** ÖNEMLÝ: HEM API CONTROLLER'LARI HEM DE MVC CONTROLLER'LARI ÝÇÝN BURASI! ***
// Hem View'larý iþleyebilen MVC Controller'lar hem de JSON/XML döndüren API Controller'lar için.
builder.Services.AddControllersWithViews();

// Cookie Authentication yapýlandýrmasý (Eðer MVC kýsmýnda kullanýcý giriþi ve yetkilendirme kullanýyorsan)
// API için farklý bir kimlik doðrulama yöntemi (örn. JWT) kullanýlýyorsa, o ayrýca yapýlandýrýlmalý veya bu kýsým çýkarýlabilir.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";      // Kullanýcý giriþ sayfasý
        options.AccessDeniedPath = "/Account/AccessDenied";  // Yetkisiz eriþim sayfasý
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Cookie ömrü
    });

// Swagger/OpenAPI Servisleri (API dökümantasyonu için)
builder.Services.AddEndpointsApiExplorer(); // Swagger UI için endpointleri keþfetmeye yarar
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AdminPanel API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header kullanýmý: 'Bearer {token}'",
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