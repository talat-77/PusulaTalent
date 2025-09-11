using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SchoolManangement.Business.ServiceRegistrations;
using SchoolManangement.DataAccess.Data;
using SchoolManangement.DataAccess.Extensions;
using SchoolManangement.Entity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace SchoolManangement.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1️⃣ Connection string environment variable'den al
            var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_URL");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("POSTGRESQL_URL environment variable bulunamadı!");
            }
            Console.WriteLine("Connection string alındı!");

            // 2️⃣ DbContext'i PostgreSQL ile ekle
            builder.Services.AddDbContext<SchoolManangementDbContext>(options =>
                options.UseNpgsql(connectionString));

            // 3️⃣ Identity konfigürasyonu
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789çğıöşüÇĞIÖŞÜ";
            })
            .AddEntityFrameworkStores<SchoolManangementDbContext>()
            .AddDefaultTokenProviders();

            // 4️⃣ JWT Authentication
            var jwtSection = builder.Configuration.GetSection("Jwt");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSection["Key"]))
                };
            });

            // 5️⃣ DataAccess ve BusinessLayer
            builder.Services.AddDataAccessLayer();
            builder.Services.AddBusinessLayer();

            // 6️⃣ Controller, Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // 7️⃣ Migration ve admin user işlemleri güvenli scope içinde
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<SchoolManangementDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                // Migration uygula
                dbContext.Database.Migrate();

                // Admin user oluştur (varsa atla)
                if (await userManager.FindByNameAsync("admin") == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@school.com",
                        FirstName = "System",
                        LastName = "Admin",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(admin, "Admin123!");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // 8️⃣ Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management API V1");
                c.RoutePrefix = "swagger";
            });

            // 9️⃣ Middleware ve Controller mapping
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
