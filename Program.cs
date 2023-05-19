using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using nWeaveTask.BL;
using nWeaveTask.DAL;
using nWeaveTask.DAL.Repositories.Products_Repo;
using System.Text;

namespace nWeaveTask;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        #region Default Services
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        #endregion

        #region Authentication
        builder.Services.AddAuthentication("default").
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

        var connectionString = builder.Configuration.GetConnectionString("nWeaveDb");
        builder.Services.AddDbContext<nWeaveContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

        builder.Services.AddScoped<IProductsRepo, ProductsRepo>(); 

        builder.Services.AddScoped<IProductsManager, ProductsManager>();


        var app = builder.Build();

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

        app.Run();
    }
}