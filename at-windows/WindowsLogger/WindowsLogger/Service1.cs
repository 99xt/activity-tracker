using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace WindowsLogger
{
    public partial class Service1 : ServiceBase
    {
        static Timer timer = new Timer(1000);
       

        public Service1()
        {
            InitializeComponent();
        }

        static List<string> schedule_Timer()
        {
            
            Console.WriteLine("Service Started");            
            ProcessLogger prostLogger = new ProcessLogger();
            List<string> latestLogger= prostLogger.LogProcess();
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            return latestLogger;

        }
        static void timer_Elapsed(Object e,ElapsedEventArgs a)
        {            
            timer.Stop();           
            schedule_Timer();
        }

        protected override void OnStart(string[] args)
        {

            while (true)
            {
                
                ProcessLogger proLoger = new ProcessLogger();
                                
                //Get the user applications set on the local machine
                List<string> logger = proLoger.LogProcess();

                //Updated Process List for every 1 second;
                List<string> latestLogger = schedule_Timer();

                //Newly Added Processes
                List<string> difference = latestLogger.Except(logger).ToList();               
                                
            }
            
        }

        
        protected override void OnStop()
        {
        }

        public void onDebug()
        {
            OnStart(null);
        }
    }
}
