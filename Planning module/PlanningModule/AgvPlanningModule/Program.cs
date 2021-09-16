using System;
using System.IO;

namespace AgvPlanningModule
{
    public class Program
    {
        private static Logging Log = new Logging();

        static void Main(string[] args)
        {
            Log.PrintAndWriteLogNewLine("\t\t\t--------------------------------");
            Log.PrintAndWriteLog("Starting planning module ...");

            AgvController agvController = new AgvController();
         
            Log.PrintAndWriteLog("Planning module is running...");

            Console.ReadKey();
        }
    }
}
