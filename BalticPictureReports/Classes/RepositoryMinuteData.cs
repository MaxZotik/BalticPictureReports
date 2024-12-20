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
    public class RepositoryMinuteData : IRepositore
    {
        private readonly ConnectionStringMSSQL connectionString;
        public DataTable TableDatas { get; set; } = null;

        public RepositoryMinuteData(DateTime currDayDT, DateTime prevDayDT)
        {
            connectionString = new ConnectionStringMSSQL();
            GetDataDb(currDayDT, prevDayDT);
        }
        

        private void GetDataDb(DateTime currDayDT, DateTime prevDayDT)
        {
            string selectQuery = $"SELECT [CnlNum], [Avg] FROM MinuteData WHERE " +
                        $"DateTime < Convert(datetime, '{currDayDT.ToString("yyyy-MM-ddTHH:mm:ss.fff")}', 127) AND " +
                        $"DateTime > Convert(datetime, '{prevDayDT.ToString("yyyy-MM-ddTHH:mm:ss.fff")}', 127)";

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
