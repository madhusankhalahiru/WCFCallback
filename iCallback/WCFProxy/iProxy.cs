using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary;
using System.ServiceModel;
using System.Xml;
using WcfServiceLibrary;
using System.ServiceModel.Description;
using EventLibrary;

namespace WCFProxy
{
    public class iProxy
    {
        WcfServiceLibrary.IService _Service;

        public bool Status
        {
            get;
            private set;
        }

        public iProxy(string strIP, string strPort)
        {
            try
            {
                
                if (EventLibCallback.oCallBack == null)
                {
                    EventLibrary.EventLibCallback.oCallBack = new EventLibrary.EventLib();
                }
                

                //EndpointAddress address = new EndpointAddress("net.tcp://" + strIP + ":" + strPort + "/TestWCFService");
                EndpointAddress address = new EndpointAddress("net.tcp://localhost:9008/TestWCFService");
                XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();
                myReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                myReaderQuotas.MaxArrayLength = Int32.MaxValue;
                myReaderQuotas.MaxDepth = Int32.MaxValue;

                NetTcpBinding oBinding = new NetTcpBinding();

                oBinding.GetType().GetProperty("ReaderQuotas").SetValue(oBinding, myReaderQuotas, null);
                oBinding.ReceiveTimeout = new TimeSpan(1, 0, 0);
                oBinding.MaxBufferSize = Int32.MaxValue;
                oBinding.MaxReceivedMessageSize = Int32.MaxValue;
                oBinding.Security.Mode = SecurityMode.None;

                DuplexChannelFactory<IService> oCF = new DuplexChannelFactory<IService>(EventLibrary.EventLibCallback.oCallBack, oBinding, address);
                foreach (OperationDescription op in oCF.Endpoint.Contract.Operations)
                {
                    var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();
                    if (dataContractBehavior != null)
                    {
                        dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
                    }
                }

                _Service = oCF.CreateChannel();

                oCF.Opened += new EventHandler(oCF_Opened);
                oCF.Closed += new EventHandler(oCF_Closed);

                ((IContextChannel)_Service).OperationTimeout = new TimeSpan(0, 20, 0);
                Status = true;
            }
            catch(Exception ex)
            {
                Status = false;
            }
        }

        void oCF_Closed(object sender, EventArgs e)
        {
            Status = false;
        }

        void oCF_Opened(object sender, EventArgs e)
        {
            Status = true;
        }

        /*public bool Ping()
        {
            return _Service.Ping();
        }

        public int Sum(int a, int b)
        {
            return _Service.Sum(a, b);
        }*/



        public void getData()
        {
            try
            {
                _Service.GetData();
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }
        }


    }
}
