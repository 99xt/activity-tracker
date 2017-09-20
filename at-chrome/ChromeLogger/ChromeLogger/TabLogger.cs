using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Automation;

using Newtonsoft.Json;


namespace ChromeLogger
{
    class JSONLOG
    {
        public string title { set; get; }
    }

    class TabLogger
    {
        Process[] chromeTabs = Process.GetProcessesByName("chrome");
        StreamWriter sw = File.AppendText(@"D:\ChromeLogger.txt");
        private List<string> jsonObjectList = new List<string>();


        public List<string> chromeTabLogger()
        {
            if (chromeTabs.Length < 0)
            {
                sw.WriteLine("Chrome is not running");
                sw.Close();
            }
            else
            {
                foreach (Process p in chromeTabs)
                {
                    if (p.MainWindowHandle == IntPtr.Zero)
                    {
                        continue;
                    }
                    //Find newTab
                    AutomationElement root = AutomationElement.FromHandle(p.MainWindowHandle);
                    Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
                    AutomationElement elmNewTab = root.FindFirst(TreeScope.Descendants, condNewTab);
                    // get the tabstrip by getting the parent of the 'new tab' button 
                    TreeWalker treewalker = TreeWalker.ControlViewWalker;
                    AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);
                    // loop through all the tabs and get the names which is the page title 
                    Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                    foreach (AutomationElement tabitem in elmTabStrip.FindAll(TreeScope.Children, condTabItem))
                    {
                        JSONLOG tab = new JSONLOG { title = tabitem.Current.Name };
                        string k = Newtonsoft.Json.JsonConvert.SerializeObject(tab);
                        jsonObjectList.Add(k);
                    }
                }

            }            
            return jsonObjectList;
        }
    }
}
