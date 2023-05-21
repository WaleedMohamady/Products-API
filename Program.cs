using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using nWeaveTask.BL;
using nWeaveTask.DAL;
using nWeaveTask.DAL.Data.Models;
using nWeaveTask.DAL.Repositories.Products_Repo;
using System.Security.Claims;
using System.Text;

namespace nWeaveTask;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        #region Services

        #region Controllers and Swagger
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        #endregion

        #region Context
        var connectionString = builder.Configuration.GetConnectionString("nWeaveDb");
        builder.Services.AddDbContext<nWeaveContext>(options => options.UseSqlServer(connectionString));
        #endregion

        #region ASP Identity
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 5;

            options.User.RequireUniqueEmail = true;

            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
        })
            .AddEntityFrameworkStores<nWeaveContext>();
        #endregion

        #region Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "default";
            options.DefaultChallengeScheme = "default";
        }).
            AddJwtBearer("default", options =>
            {
                var secretKey = builder.Configuration.GetValue<string>("SecretKey");
                var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKey);
                var key = new SymmetricSecurityKey(secretKeyInBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key
                };
            });
        #endregion

        #region Authorization

        builder.Services.AddAuthorization(options =>
        {
        options.AddPolicy("AllowAdminAndManager", policy =>
            policy
            .RequireClaim(ClaimTypes.Role, "Administrator", "Manager"));
        });

        #endregion

        #region DPI
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

        builder.Services.AddScoped<IProductsRepo, ProductsRepo>();

        builder.Services.AddScoped<IProductsManager, ProductsManager>();
        #endregion

        #endregion

        var app = builder.Build();

        #region MiddleWares
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        #endregion

        app.Run();
    }
}