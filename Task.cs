using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KeyValueStore = System.Collections.Generic.Dictionary<object, object>;

namespace ThreadPoolExample
{
    public class Task
    {
        /****************** GLOBAL VARIABLES ******************************/
        private List<KeyValueStore> _list;
        private WaitCallback _callBackFunction;
        /*****************************************************************/

        public Task(List<KeyValueStore> list, WaitCallback callBackFunction)
        {
            _list = list;
            _callBackFunction = callBackFunction;
        }
        private Task() { }
        //  public static List<List<T>> Split<T>(List<T> self )
        // Wrapper method for use with thread pool.
        public void Start()
        {
            // Configure and launch threads using ThreadPool:
            Console.WriteLine("launching {0} tasks...", _list.Count);
            int index = 0;

            //var split = ListSplitterForThreading.Split(list, 60);
            var split = ListSplitterForThreading.Split(_list);

            List<List<ManualResetEvent>> AllDoneEvents = new List<List<ManualResetEvent>>();
            foreach (List<KeyValueStore> tmpList in split)
            {
                List<ManualResetEvent> doneEvents = new List<ManualResetEvent>();

                ManualResetEvent tmpDoneEvent;
                foreach (KeyValueStore item in tmpList)
                {
                    index = _list.IndexOf(item);

                    tmpDoneEvent = new ManualResetEvent(false);
                    doneEvents.Add(tmpDoneEvent);

                    item.Add("threadContext", index);
                    item.Add("DoneEvent", tmpDoneEvent);

                    ThreadPool.QueueUserWorkItem(_callBackFunction, item);
                }
                index = 0;
                // Wait for all threads in pool to finish... 
                AllDoneEvents.Add(doneEvents);
            }

            foreach (var doner in AllDoneEvents)
            {
                WaitHandle.WaitAll(doner.ToArray());
            }
        }


    }
}
