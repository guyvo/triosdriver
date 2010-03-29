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
         
        private HttpListener _httpListener;
        protected static AutoResetEvent listenForNextRequest = new AutoResetEvent(false);
        protected String htmlErrorString = 
            "<html><h1>Bad request because you didn't say the magic word !<//h1><//html>";

        protected FileStream myLog;
        protected TextWriter writeLog;

        public WebServer(string port)
        {
            try
            {
                this._httpListener = new HttpListener();
                _httpListener.Prefixes.Add("http://*:" + port + "/");
                myLog = new FileStream(".\\serverlog.log", FileMode.Append, FileAccess.Write);
                writeLog = new StreamWriter(myLog);
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
                myLog.Close();
            }
        }

        public void Start() {
            try
            {
                _httpListener.Start();
                System.Threading.ThreadPool.QueueUserWorkItem(listen);
            }
            catch (Exception e)
            {
                _httpListener.Close();
                MessageBox.Show(e.Message);     
            }
        }

        public void Stop() {
            try
            {
                _httpListener.Stop();
            }
            catch (Exception e)
            {
                return;
            }
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

        string amountOfCortex;

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
            

            amountOfCortex = context.Request.Headers.Get("cortex");

            if ( amountOfCortex == null || amountOfCortex.Equals("") || Convert.ToInt32(amountOfCortex) > 4 || Convert.ToInt32(amountOfCortex) < 1)
            {
                TextWriter writer = new StreamWriter(context.Response.OutputStream);

                writeLog.WriteLine("BAD REQUEST: " + context.Request.RemoteEndPoint.Address.ToString() + " " + context.Request.UserHostName);
                writeLog.Flush();
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 400;
                context.Response.ContentLength64 = htmlErrorString.Length;
                context.Response.AddHeader("status", "bad request");
                writer.Write(htmlErrorString);
                writer.Close();
                context.Request.InputStream.Close();
                context.Response.OutputStream.Close();
                
                return;
            }

            writeLog.WriteLine("REQUEST OK: " + context.Request.RemoteEndPoint.Address.ToString() + " " + context.Request.UserHostName + " " + context.Request.HttpMethod);
            writeLog.Flush();

            Array.Clear(buffer, 0, buffer.Length);
            
            if (context.Request.HttpMethod == "GET")
            {
                TextWriter writer = new StreamWriter(context.Response.OutputStream);

                try
                {
                    serialPort.Open(_serialportname);

                    if (Convert.ToInt32(amountOfCortex) > 0)
                    {
                        buffer[0] = 2;
                        buffer[1] = 0x10;
                        buffer[2] = 0;
                        buffer[3] = 84;
                    }

                    if (Convert.ToInt32(amountOfCortex) > 1)
                    {
                        buffer[256] = 2;
                        buffer[257] = 0x30;
                        buffer[258] = 0;
                        buffer[259] = 84;
                    }

                    if (Convert.ToInt32(amountOfCortex) > 2)
                    {
                        buffer[512] = 2;
                        buffer[513] = 0x50;
                        buffer[514] = 0;
                        buffer[515] = 84;
                    }

                    if (Convert.ToInt32(amountOfCortex) > 3)
                    {
                        buffer[768] = 2;
                        buffer[769] = 0x70;
                        buffer[770] = 0;
                        buffer[771] = 84;
                    }

                    serialPort.Send(buffer);

                    theData = ProcessRefresh();

                    context.Response.ContentLength64 = theData.Length;
                    context.Response.ContentType = "text/xml";
                    writer.Write(theData);
                    writer.Flush();

                }
                catch (Exception e)
                {
                    MessageBox.Show("GET failed :" + e.Message);
                }
                finally
                {
                    writer.Close();
                    context.Response.Close();
                    serialPort.Close();
                }

            }
            else if (context.Request.HttpMethod == "POST")
            {

                try
                {
                    if (Convert.ToInt32(amountOfCortex) > 0)
                    {
                        buffer[0] = 1;
                        buffer[1] = 0x20;
                        buffer[2] = 0;
                        buffer[3] = 84;
                    }

                    if (Convert.ToInt32(amountOfCortex) > 1)
                    {
                        buffer[256] = 1;
                        buffer[257] = 0x40;
                        buffer[258] = 0;
                        buffer[259] = 84;
                    }

                    if (Convert.ToInt32(amountOfCortex) > 2)
                    {
                        buffer[512] = 1;
                        buffer[513] = 0x60;
                        buffer[514] = 0;
                        buffer[515] = 84;
                    }

                    if (Convert.ToInt32(amountOfCortex) > 3)
                    {
                        buffer[768] = 1;
                        buffer[769] = 0x80;
                        buffer[770] = 0;
                        buffer[771] = 84;
                    }

                    ProcessUpdate(context.Request.InputStream);

                    serialPort.Open(_serialportname);
                    serialPort.Send(buffer);

                    context.Response.ContentType = "text/xml";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = 0;
                    context.Response.AddHeader("status", "done");
                    context.Response.OutputStream.Write(buffer, 0, 0);
                }
                catch (Exception e)
                {
                    MessageBox.Show("POST failed :" + e.Message);
                }
                finally
                {
                    serialPort.Close();
                }
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
            Cortex[] cortex = new Cortex[Convert.ToInt32(amountOfCortex)];
            ushort[] vals = new ushort[6];
            string xmlData;

            for ( int c = 0, idx = 0 ; c < cortex.Length ; c++ , idx += 256 ){
                cortex[c] = new Cortex("Cortex" + (c+1));
                triosModel.addCortex(cortex[c]);
                Buffer.BlockCopy(buffer, 4 + idx, vals, 0, 12);
                cortex[c].addLight(new Light("OUT1", vals));
                Buffer.BlockCopy(buffer, 16 + idx, vals, 0, 12);
                cortex[c].addLight(new Light("OUT2", vals));
                Buffer.BlockCopy(buffer, 28 + idx, vals, 0, 12);
                cortex[c].addLight(new Light("OUT3", vals));
                Buffer.BlockCopy(buffer, 40 + idx, vals, 0, 12);
                cortex[c].addLight(new Light("OUT4", vals));
                Buffer.BlockCopy(buffer, 52 + idx, vals, 0, 12);
                cortex[c].addLight(new Light("OUT5", vals));
                Buffer.BlockCopy(buffer, 64 + idx, vals, 0, 12);
                cortex[c].addLight(new Light("OUT6", vals));
                Buffer.BlockCopy(buffer, 76 + idx, vals, 0, 12);
                cortex[c].tempsensor = vals[0];
                cortex[c].watchdog = vals[1];
                cortex[c].toggle = vals[2];
                cortex[c].dimmer = vals[3];
                cortex[c].hours = vals[4];
                cortex[c].masks = vals[5];
            }
        
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

            for (int c = 0 , idx = 0; c < triosModel.arrayCortex.Count; c++ , idx +=256 )
            {
                Cortex name = (Cortex)triosModel.arrayCortex[c];

                for (int i = 0; i < name.arrayLights.Count; i++)
                {
                    Light light = (Light)name.arrayLights[i];

                    vals[0] = light.val;
                    vals[1] = light.min;
                    vals[2] = light.max;
                    vals[3] = light.step;
                    vals[4] = light.pinin;
                    vals[5] = light.pinout;

                    Buffer.BlockCopy(vals, 0, buffer, 4 + (i * 12) + idx , 12);
                }

                vals[0] = name.tempsensor;
                vals[1] = name.watchdog;
                vals[2] = name.toggle;
                vals[3] = name.dimmer;
                vals[4] = name.hours;
                vals[5] = name.masks;

                Buffer.BlockCopy(vals, 0, buffer, 76 + idx, 12);

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
