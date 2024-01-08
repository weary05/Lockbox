using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockBoxWindows
{
    public class DataHandling
    {
        public string RetrieveData()
        {
            string fileName = Directory.GetCurrentDirectory() + @"\Data.txt";
            StreamReader reader = new StreamReader(fileName);
            return reader.ReadToEnd();
        }
        public void SaveData(string data)
        {
            string fileName = Directory.GetCurrentDirectory() + @"\Data.txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter writer = new StreamWriter(fileName);
            writer.Write(data);
        }
    }
}
