using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WcfServiceLibrary
{
    public class Service : IService
    {
        public void GetData()
        {

            //for (int i = 0; i <= 5; i++)
            //{
            //    Thread.Sleep(10000);
            //}
            String s = "Task completed: " + DateTime.Now.ToString();
            OperationContext.Current.GetCallbackChannel<IServiceCallback>().GetDataCompleted(s);
        }
    }
}
