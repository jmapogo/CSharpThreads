using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using KeyValueStore = System.Collections.Generic.Dictionary<object, object>;

namespace ThreadPoolExample
{
 
    class Program
    {

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        static void Main()
        {
             

           

 
            List<KeyValueStore> lst = new List<KeyValueStore>();
            for (var i = 1; i <  21; i++)
            {
                KeyValueStore kv = new KeyValueStore();
                kv.Add("name", "johny " + i);
                lst.Add(kv);
            }

             WaitCallback callBackFunction = new WaitCallback(delegate(object objVars)
             {
                 KeyValueStore dVars = (KeyValueStore)objVars;
                 int threadIndex = (int)dVars["threadContext"];
                 ManualResetEvent _doneEvent;
                 
               _doneEvent = (ManualResetEvent)dVars["DoneEvent"];
                  
 
                 string name =  dVars["name"].ToString();
                 Console.WriteLine("my name is " + name);
                  
                  
                 _doneEvent.Set();
             });

             Task t = new Task(lst,callBackFunction);
             t.Start();
             
            
            Console.ReadKey();
        }
             
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return new Random().Next(min, max);
            }
        }

    }

}
