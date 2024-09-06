using Microsoft.EntityFrameworkCore;
using UMS.DAL.Models;

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApplication2.Mapper;
using UMS.DAL.Interfaces;
using UMS.DAL.Services;
using UMS.Core.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UMS.BL.Configuration;
using Microsoft.VisualBasic;
using System.Text;
using UMS.Core;
using DK.Web.Middlewares;
using UMS.BL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Context
var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<ApplicationDbContext>(item => item.UseSqlServer(config.GetConnectionString("test")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// adding session services to app
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
    option.Cookie.MaxAge = TimeSpan.FromDays(1);
});

// Add JWT authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});

// adding services of mail
builder.Services.Configure<SMTPConfig>(builder.Configuration.GetSection("SmtpSettings"));

// Add Repository scope
//builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));    // Generic Repository
builder.Services.AddScoped(typeof(BaseRepository<>));    // Generic Repository
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAdminServices,AdminServices>();
builder.Services.AddScoped<IEmailService, EmailService>();

// adding JWT credentials
builder.Services.AddAuthentication(options =>
{
    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = "CookieAuthentication";
    options.DefaultChallengeScheme = "CookieAuthentication";
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        //ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
})
.AddCookie("CookieAuthentication", config =>
{
    config.Cookie.Name = "UserLoginCookie"; // Name of cookie
    config.LoginPath = "/Auth/Login"; // Path for the redirect to user login page
    config.AccessDeniedPath = "/Home/AccessDenied"; // Path for access denied
    config.Cookie.HttpOnly = true; // Additional option
    config.ExpireTimeSpan = TimeSpan.FromDays(30); // Additional option
    config.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        Constant.ADMIN_POLICY,
        policy => policy.RequireRole("Admin")
    );

    options.AddPolicy(
        Constant.USER_POLICY,
        policy => policy.RequireRole("User")
    );

    options.AddPolicy(
        Constant.ADMIN_USER_POLICY,
        policy => policy.RequireAssertion(context => context.User.IsInRole("Admin") || context.User.IsInRole("Admin"))
    );
});

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

// using session in app
app.UseSession();

app.UseRouting();
app.UseCookiePolicy();

app.UseMiddleware<AuthMiddleware>();

app.UseRouting();

app.UseAuthentication(); // Add this before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
