using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalticPictureReports.Classes
{
    public static class DirectoryFile
    {
        public static void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static void CheckFile(string path)
        {
            if (File.Exists(path) == false)
            {
                File.Create(path).Close();
            }
        }
    }
}
