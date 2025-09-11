using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.DataAccess.Data
{
    public class SchoolManangementDbContextFactory : IDesignTimeDbContextFactory<SchoolManangementDbContext>
    {
        public SchoolManangementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolManangementDbContext>();

            var connectionString = "Host=localhost;Port=5433;Database=SchoolManangement;Username=postgres;Password=123456789";

            optionsBuilder.UseNpgsql(connectionString);
            return new SchoolManangementDbContext(optionsBuilder.Options);
        }
    }
}
