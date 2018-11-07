using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EventLibrary;
using WCFProxy;
using System.Threading;

namespace iCallback
{
    public partial class Form1 : Form
    {

        iProxy oProxy = null;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

        public Form1()
        {
            InitializeComponent();
            
            
            oProxy = new iProxy("a", "b");
            SubscribeEvents();
            //EventLibCallback.oCallBack = new EventLibCallback.
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void SubscribeEvents()
        {
            try
            {
                //iATMBioMetricEventLib.BioMetricCallBackImple.oCallBack.OpenDeviceCompleted_Event += new iATMBioMetricEventLib.EventsLibrary.OpenDeviceCompleted(oCallBack_GetDataCompleted_Event);
                EventLibCallback.oCallBack.GetDataCompleted_Event += new EventLib.GetData_Completed(oCallBack_GetDataCompleted_Event);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void oCallBack_GetDataCompleted_Event(String str)
        {
            try
            {
                label1.Text = "Device opened completed event fired. Result: " + str;
            }
            catch (Exception)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //backgroundWorker1.RunWorkerAsync();

            if (oProxy != null)
            {
                oProxy.getData();
            }

            MessageBox.Show("Test");
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker helperBW = sender as System.ComponentModel.BackgroundWorker;
            //int arg = (int)e.Argument;


            try
            {
                if (oProxy != null)
                {
                    oProxy.getData();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }


            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private object BackgroundProcessLogicMethod(System.ComponentModel.BackgroundWorker helperBW, int arg)
        {
            int result = 111;
            Thread.Sleep(6000);
            MessageBox.Show("I was doing some work in the background.");
            return result;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled) MessageBox.Show("Operation was canceled");
            else if (e.Error != null) MessageBox.Show(e.Error.Message);
            else { 
                
            }
        }

    }
}
