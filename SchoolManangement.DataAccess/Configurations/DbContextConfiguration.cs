using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.DataAccess.Configurations
{
    static class DbContextConfiguration
    {
        public static string GetConnectionString =>
            Environment.GetEnvironmentVariable("POSTGRESQL_URL")
            ?? throw new InvalidOperationException("POSTGRESQL_URL environment variable tanımlanmamış!");
    }
}
