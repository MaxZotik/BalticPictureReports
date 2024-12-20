using BalticPictureReports.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalticPictureReports.Classes
{
    public class ConnectionStringMSSQL : IConnectDb
    {
        private string path = "";
        public string ConnectionString { get; set; } = "";

        public ConnectionStringMSSQL()
        {
            path = Directory.GetCurrentDirectory() + @"\FileResourses\db_name.txt";
            InitConnectionString();
        }

        private void InitConnectionString()
        {
            string[] read_result_array = File.ReadAllLines(path, Encoding.Default);

            ConnectionString = $@"Data Source={read_result_array[0]};Initial Catalog={read_result_array[1]};User ID={read_result_array[2]};Password={read_result_array[3]}";
        }
    }
}
