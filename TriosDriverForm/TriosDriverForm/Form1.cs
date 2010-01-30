
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Web;
using System.Net;
using System.Xml;

namespace TriosDriverForm
{
    public partial class Form1 : Form 
    {
       

        ManualResetEvent manualEvent = new ManualResetEvent(false);
        WebServerImpl web = new WebServerImpl("80");
        
        byte[] buffer = new byte[4096];
        ushort[] vals = new ushort[42];
        int idx = 0;

        

        public Form1()
        {
            InitializeComponent();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int temp;
            
            
            temp = serialPort1.Read(buffer, idx, buffer.Length - idx);
            idx += temp;
            if (idx == 4096)
                manualEvent.Set();
          
        }

        private void bOpenSerialPort_Click(object sender, EventArgs e)
        {
            web.Start();

            try
            {
                serialPort1.PortName = textComPort.Text.Trim();
                serialPort1.Open();
                rIsConnected.Checked = serialPort1.IsOpen;
                bSendBuffer.Enabled = serialPort1.IsOpen;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(" Invalid operation " , "Serial port", MessageBoxButtons.OK);
            }
            finally
            {
               
            }

        }

        private void bSendBuffer_Click(object sender, EventArgs e)
        {
           
            buffer[0] = 2;
            buffer[1] = 16;
            buffer[2] = 0;
            buffer[3] = 84;
            
            serialPort1.Write(buffer, 0, buffer.Length);
            manualEvent.WaitOne();
            manualEvent.Reset();
            MessageBox.Show(Convert.ToString(idx));
            idx = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }

        private void bReadBytes_Click(object sender, EventArgs e)
        {
            int l = 0;
            
            string[] list = new string[4096];

           
            Buffer.BlockCopy(buffer, 4, vals, 0, 84);
                        
            foreach (ushort b in vals){
                list[l] = Convert.ToString(l) + " -> " + Convert.ToString(b,10);
                l++;
            }

            textStatus.Lines = list;

            web.Stop();

        }
    }
}
