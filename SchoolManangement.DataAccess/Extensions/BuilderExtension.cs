using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolManangement.DataAccess.Data;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using SchoolManangement.DataAccess.UnitOfWorks.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.DataAccess.Extensions
{
    public static class BuilderExtension
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SchoolManangementDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
