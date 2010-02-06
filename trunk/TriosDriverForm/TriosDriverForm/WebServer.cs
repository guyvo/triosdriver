using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Xml.Serialization;
using System.Xml;

namespace TriosDriverForm
{
    abstract public class WebServer
    {
        protected static AutoResetEvent listenForNextRequest = new AutoResetEvent(false); 
        private HttpListener _httpListener;

        public WebServer(string port)
        {
            this._httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://*:" + port + "/");
         }

        public void Start() {
            _httpListener.Start();
            System.Threading.ThreadPool.QueueUserWorkItem(listen);
        }

        public void Stop() {
            _httpListener.Stop();
        }

        private void listen(object state)
        {
            while (_httpListener.IsListening) {
                _httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), _httpListener);
                listenForNextRequest.WaitOne();
            }
        
        }

        private void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = result.AsyncState as HttpListener;
            HttpListenerContext context = null;

            try
            {
                context = listener.EndGetContext(result);
            }
            catch (Exception )
            {
                return;
            }

            handleRequest(context);

            listenForNextRequest.Set();
        
        }

        protected abstract void handleRequest(HttpListenerContext context);
       
    }
    //
    public class WebServerImpl : WebServer ,ISerial
    {
        const int maxBUFFER = 4096;

        ISerial serialPort;

        SerialPort theSerialPort;
        ManualResetEvent manualEvent = new ManualResetEvent(false);
        string _serialportname;

        byte[] buffer = new byte[maxBUFFER];
        int idx = 0;

        public WebServerImpl(string webportname, string serialportname)
            : base(webportname)
        {
            _serialportname = serialportname;
            serialPort = this;
        } 

        protected override void handleRequest(HttpListenerContext context)
        {
            string theData;

            if (context.Request.HttpMethod == "GET")
            {
                TextWriter writer = new StreamWriter(context.Response.OutputStream);
                
                serialPort.Open(_serialportname);

                buffer[0] = 2;
                buffer[1] = 16;
                buffer[2] = 0;
                buffer[3] = 84;

                serialPort.Send(buffer);

                theData = ProcessRefresh();
                
                context.Response.ContentLength64 = theData.Length;
                context.Response.ContentType = "text/xml";
                writer.Write(theData);
                writer.Flush();
                writer.Close();
                context.Response.Close();

                serialPort.Close();

            }

            else if (context.Request.HttpMethod == "POST")
            {
               
                ProcessUpdate(context.Request.InputStream);

                buffer[0] = 1;
                buffer[1] = 32;
                buffer[2] = 0;
                buffer[3] = 84;

                serialPort.Open(_serialportname);
                serialPort.Send(buffer);

                context.Response.ContentType = "text/xml";
                context.Response.StatusCode = 200;
                context.Response.ContentLength64 = 0;
                
                context.Response.Close();

                serialPort.Close();
            }
 
        }

        #region ISerial Members

        void ISerial.Open(string port)
        {
            theSerialPort = new SerialPort(port);
            theSerialPort.BaudRate = 4500000;
            theSerialPort.WriteBufferSize = maxBUFFER;
            theSerialPort.ReadBufferSize = maxBUFFER;
            theSerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            theSerialPort.Open();
        }

        void ISerial.Send(byte[] data)
        {
            if (data.Length <= maxBUFFER)
            {
                theSerialPort.Write(data, 0, data.Length);
                manualEvent.WaitOne();
                idx = 0;
                manualEvent.Reset();
            }
        }

        void ISerial.Close()
        {
            theSerialPort.Close();
        }

        #endregion
        
        private string ProcessRefresh()
        {
            TriosModel triosModel = new TriosModel();
            Cortex[] cortex = new Cortex[4];
            ushort[] vals = new ushort[6];
            string xmlData;

            // 1
            cortex[0] = new Cortex("Cortex1");
            triosModel.addCortex(cortex[0]);
            Buffer.BlockCopy(buffer, 4, vals, 0, 12);
            cortex[0].addLight(new Light("OUT1", vals));
            Buffer.BlockCopy(buffer, 16, vals, 0, 12);
            cortex[0].addLight(new Light("OUT2", vals));
            Buffer.BlockCopy(buffer, 28, vals, 0, 12);
            cortex[0].addLight(new Light("OUT3", vals));
            Buffer.BlockCopy(buffer, 40, vals, 0, 12);
            cortex[0].addLight(new Light("OUT4", vals));
            Buffer.BlockCopy(buffer, 52, vals, 0, 12);
            cortex[0].addLight(new Light("OUT5", vals));
            Buffer.BlockCopy(buffer, 64, vals, 0, 12);
            cortex[0].addLight(new Light("OUT6", vals));

            XmlSerializer xmlModel = new XmlSerializer(typeof(TriosModel));
            TextWriter writer = new StreamWriter(@"c:\refresh.xml");
            xmlModel.Serialize(writer, triosModel);
            writer.Flush();
            writer.Close();

            TextReader reader = new StreamReader(@"c:\refresh.xml");
            xmlData = reader.ReadToEnd();
            reader.Close();

            return xmlData;      
        }

        private void ProcessUpdate(Stream post)
        {
            ushort[] vals = new ushort[6];

            XmlSerializer xmlModel = new XmlSerializer(typeof(TriosModel));
            TriosModel triosModel = (TriosModel)xmlModel.Deserialize(post);


            Cortex name = (Cortex)triosModel.arrayCortex[0];

            for (int i = 0; i < name.arrayLights.Count; i++) 
            {
                Light light = (Light) name.arrayLights[i];

                vals[0] = light.val;
                vals[1] = light.min;
                vals[2] = light.max;
                vals[3] = light.step;
                vals[4] = light.pinin;
                vals[5] = light.pinout;

                Buffer.BlockCopy(vals, 0, buffer , 4 + (i * 12) , 12);
            }
           
            
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int temp;

            temp = theSerialPort.Read(buffer, idx, buffer.Length - idx);
            idx += temp;
            if (idx == buffer.Length)
                manualEvent.Set();
        }

        
    }
}
