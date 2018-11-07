using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Xml;
using System.ServiceModel.Description;
using WcfServiceLibrary;

namespace WCFServer
{
    public partial class Form1 : Form
    {
        ServiceHost Host;
        string BindingString;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BindingString = "net.tcp://localhost:9008/TestWCFService";
            Host = new ServiceHost(typeof(Service));

            NetTcpBinding oBinding = new NetTcpBinding();

            XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();
            myReaderQuotas.MaxStringContentLength = Int32.MaxValue;
            myReaderQuotas.MaxArrayLength = Int32.MaxValue;
            myReaderQuotas.MaxDepth = Int32.MaxValue;


            oBinding.GetType().GetProperty("ReaderQuotas").SetValue(oBinding, myReaderQuotas, null);
            oBinding.SendTimeout = new TimeSpan(0, 1, 0);
            oBinding.MaxBufferSize = Int32.MaxValue;
            oBinding.MaxReceivedMessageSize = Int32.MaxValue;
            oBinding.Security.Mode = SecurityMode.None;


            Host.AddServiceEndpoint(typeof(IService), oBinding, BindingString);
            Host.Opened += new EventHandler(Host_Opened);
            Host.Closed += new EventHandler(Host_Closed);


            foreach (ServiceEndpoint ep in Host.Description.Endpoints)
            {
                foreach (OperationDescription op in ep.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior dataContractBehavior =
                       op.Behaviors.Find<DataContractSerializerOperationBehavior>()
                            as DataContractSerializerOperationBehavior;
                    if (dataContractBehavior != null)
                    {
                        dataContractBehavior.MaxItemsInObjectGraph = Int32.MaxValue;
                    }
                }
            }
            try
            {
                Host.Open();
                Console.WriteLine("");
            }
            catch(Exception ex)
            {
            }
        }

        void Host_Closed(object sender, EventArgs e)
        {
            label1.Text = "Service Stopped";
            label1.ForeColor = Color.Maroon;
        }

        void Host_Opened(object sender, EventArgs e)
        {
            label1.Text = "Service Started";
            label1.ForeColor = Color.DarkGreen;
        }


    }
}
