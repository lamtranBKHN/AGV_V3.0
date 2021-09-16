using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AgvPlanningModule
{
    class Logging
    {
        private string m_path = "Logs";
        public Logging() 
        {         
            bool exists = System.IO.Directory.Exists(m_path);

            if (!exists)
                System.IO.Directory.CreateDirectory(m_path);
        }
        ~Logging() { }
        public void PrintAndWriteLog(string logData)
        {
            try
            {
                string currentTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                logData = $"{currentTime}\t{logData}\n";
                string today = DateTime.Now.ToString("MMddyyyy");
                File.AppendAllText($"{m_path}/{today}.log", logData);
                Console.WriteLine(logData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }            
        }
        public void PrintAndWriteLogNewLine(string logData)
        {          
            try
            {
                string currentTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                logData = $"\n{currentTime}\t{logData}\n";
                string today = DateTime.Now.ToString("MMddyyyy");
                File.AppendAllText($"{m_path}/{today}.log", logData);
                Console.WriteLine(logData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
