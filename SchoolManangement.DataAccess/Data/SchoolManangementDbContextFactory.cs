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

            var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_URL")
                                   ?? throw new InvalidOperationException("POSTGRESQL_URL environment variable tanımlanmamış!");

            optionsBuilder.UseNpgsql(connectionString);
            return new SchoolManangementDbContext(optionsBuilder.Options);
        }
    }

}
