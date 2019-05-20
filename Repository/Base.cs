using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using System.Data;
using Entities;

namespace Repository
{
    public static class Base
    {
        private static string connectionString;
        private static IConfiguration config;

        internal static IDbConnection Connection
        {
            get
            {
              IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();
                SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
                return new NpgsqlConnection(configuration.GetValue<string>("DBInfo:ConnectionString"));
            }
        }

       
    }
}
