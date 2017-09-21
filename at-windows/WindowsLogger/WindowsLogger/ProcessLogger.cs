using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Newtonsoft.Json;


namespace WindowsLogger
{
    class JSONProcess
    {
        public string processName { get; set; }
        public string processTitle { get; set; }
    }

    class ProcessLogger
    {
        private Process[] processList = Process.GetProcesses();        
        private List<string> jsonObjectList = new List<string>();

        
        public List<string> LogProcess()
        {
            foreach (Process p in processList)
            {                
                try
                {
                    if (p.MainWindowHandle != IntPtr.Zero)
                    {
                        JSONProcess pro = new JSONProcess { processName = p.ProcessName, processTitle = p.MainWindowTitle };
                        string k = Newtonsoft.Json.JsonConvert.SerializeObject(pro);
                        jsonObjectList.Add(k);
                    }

                    p.EnableRaisingEvents = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errror");
                }                
            }           
            return jsonObjectList;
        }
    }
}
