using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostPhotographerBlog.Data
{
    public class Settings
    {
        private static string _connectionString;
        private static string _mode;
        
        public static void SetConnectionString(string connectionName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public static string GetConnection()
        {
            return _connectionString;
        }

        public static string GetMode()
        {
            if (string.IsNullOrEmpty(_mode))
            {
                _mode = ConfigurationManager.AppSettings["Mode"].ToString();
            }

            return _mode;
        }
    }
}
