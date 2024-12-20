using BalticPictureReports.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalticPictureReports.Classes
{
    public class RepositoryConfigLimits : IRepositore
    {
        private readonly ConnectionStringMSSQL connectionString;

        public DataTable TableDatas { get; set; } = null;

        public RepositoryConfigLimits()
        {
            connectionString = new ConnectionStringMSSQL();
            GetDataDb();
        }
        

        private void GetDataDb()
        {
            string selectQuery = $"SELECT [CnlNum], [Limit] FROM ConfigLimits";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString.ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    TableDatas = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }

        }
    }
}
