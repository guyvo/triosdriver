using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.IO;

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
    public class WebServerImpl : WebServer
    {
        public WebServerImpl(string port): base ( port){
          
        } 

        protected override void handleRequest(HttpListenerContext context)
        {
            
            string theData;

            if (context.Request.HttpMethod == "GET")
            {
                TextReader reader = new StreamReader(@"C:\model.xml");
                System.IO.TextWriter w = new System.IO.StreamWriter(context.Response.OutputStream);

                theData = reader.ReadToEnd(); 
                context.Response.ContentLength64 = theData.Length;
                context.Response.ContentType = "text/xml";
                w.Write(theData);
                w.Flush();
                
                reader.Close();
                w.Close();
                context.Response.Close();
            }
            else if (context.Request.HttpMethod == "POST")
            {
                
                TextWriter w = new StreamWriter(@"C:\fx.xml");
                TextReader reader = new StreamReader(context.Request.InputStream);

                w.Write(reader.ReadToEnd());
                w.Close();

                context.Response.ContentType = "text/xml";
                context.Response.StatusCode = 200;
                context.Response.ContentLength64 = 0;
                
                context.Response.Close();
            }
 
        }
    }
}
