using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EnvironmentManager
{
    public class Connection
    {
        private static Connection connection = null;

        public static Connection CreateConnectionInstance
        {
            get
            {
                if (connection == null)
                {
                    connection = new Connection();
                }

                return connection;
            }
        }

        public IConfiguration configuration = null;

        public IConfiguration SetConfiguration()
        {
            if (configuration == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile($"EnvironmentManager/appsettings.json", true, true);
                configuration = builder.Build();
            }
            return configuration;
        }


        public string connString { get; set; }

        public Connection()
        {
            SetConfiguration();

            connString = configuration.GetValue<string>("StorageAccount:connString");
        }
    }
}
