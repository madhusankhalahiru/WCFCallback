using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary;

namespace EventLibrary
{
    public class EventLib : IServiceCallback
    {
        public delegate void GetData_Completed(String str);
        public event GetData_Completed GetDataCompleted_Event;

        public EventLib() { }


        public void GetDataCompleted(String str)
        {
            try
            {
                if (GetDataCompleted_Event != null)
                {
                    GetDataCompleted_Event(str);
                }
            }
            catch
            {
            }
        }

    }
}
