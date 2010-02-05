
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
using System.Xml.Serialization;
using System.IO;

namespace TriosDriverForm
{
    public partial class Form1 : Form 
    {
       

        ManualResetEvent manualEvent = new ManualResetEvent(false);
        WebServerImpl web = new WebServerImpl("8080");
        
        byte[] buffer = new byte[4096];
        ushort[] vals = new ushort[6];
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

            TriosModel c = new TriosModel();
            Cortex[] cortex = new Cortex[4];

            // 1
            cortex[0] = new Cortex("Cortex1");
            c.addCortex(cortex[0]);
            Buffer.BlockCopy(buffer, 4, vals, 0, 12);
            cortex[0].addLight(new Light("OUT1",vals));
            Buffer.BlockCopy(buffer, 16, vals, 0, 12);
            cortex[0].addLight(new Light("OUT2",vals));
            Buffer.BlockCopy(buffer, 28, vals, 0, 12);
            cortex[0].addLight(new Light("OUT3",vals));
            Buffer.BlockCopy(buffer, 40, vals, 0, 12);
            cortex[0].addLight(new Light("OUT4",vals));
            Buffer.BlockCopy(buffer, 52, vals, 0, 12);
            cortex[0].addLight(new Light("OUT5",vals));
            Buffer.BlockCopy(buffer, 64, vals, 0, 12);
            cortex[0].addLight(new Light("OUT6",vals));
  
            XmlSerializer s = new XmlSerializer(typeof(TriosModel));
            System.IO.TextWriter w = new System.IO.StreamWriter(@"C:\model.xml");
           
            s.Serialize(w, c);


            w.Close();
            TextReader reader = new StreamReader(@"C:\fx.xml");
            TriosModel model = (TriosModel)  s.Deserialize(reader);
            reader.Close();

            //web.Stop();

        }
    }
}
