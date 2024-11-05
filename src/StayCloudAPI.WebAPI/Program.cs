using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StayCloudAPI.Application.Interfaces;
using StayCloudAPI.Application.Interfaces.Content.IAuth;
using StayCloudAPI.Application.Interfaces.Content.ICloudinary;
using StayCloudAPI.Application.Interfaces.Content.IHotel;
using StayCloudAPI.Application.Interfaces.Content.IRoom;
using StayCloudAPI.Application.Mappings;
using StayCloudAPI.Core.ConfigOptions;
using StayCloudAPI.Core.Domain.Identity;
using StayCloudAPI.Infrastructure;
using StayCloudAPI.Infrastructure.Implements.Content.AuthImplement;
using StayCloudAPI.Infrastructure.Implements.Content.CloudinaryImplement;
using StayCloudAPI.Infrastructure.Implements.Content.HotelImplement;
using StayCloudAPI.Infrastructure.Implements.Content.RoomImplement;
using StayCloudAPI.Infrastructure.SeedWorks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("StayCloudDB");

#region Config custom
builder.Services.AddDbContext<StayCloudAPIDbContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
    options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<StayCloudAPIDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
    options.User.RequireUniqueEmail = true;
});
#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

// Add configure
builder.Services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
builder.Services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

// Add services scoped
builder.Services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
