using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BricksMeatballs.BricksMeatballs.Models
{
    /// <summary>
    /// Author:Huang Chaoshan, Lim Pei Yan
    /// The DBConnection defines the database connection string
    /// </summary>
    public class DBConnection
    {
        public string ConnectionString { get; set; }

        public DBConnection(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
