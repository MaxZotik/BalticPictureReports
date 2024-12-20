using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalticPictureReports.Classes
{
    public static class Log
    {
        private static readonly string logPath = @"C:\Program Files\SCADA\Reports\DailyReports\Program\Log.txt";
        private static readonly string logPathDirectory = @"C:\Program Files\SCADA\Reports\DailyReports\Program";

        static Log()
        {
            DirectoryFile.CheckDirectory(logPathDirectory);
            DirectoryFile.CheckFile(logPath);
        }

        public static void WriteLog(string text)
        {           
            File.AppendAllText(logPath, DateTime.Now.ToString() + $" - {text}" + Environment.NewLine);
        }


    }
}
