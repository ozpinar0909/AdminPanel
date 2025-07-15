using AdminPanel.BLL.Managers;
using AdminPanel.BLL.Service;
using AdminPanel.DAL.Context;
using AdminPanel.DAL.Interfaces;
using AdminPanel.DAL.Repositories;
using AdminPanel.Webapi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore; // Add this using directive at the top of the file  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection  
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>(); // Updated to use the recommended method  
builder.Services.AddFluentValidationAutoValidation();
// Swagger  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
