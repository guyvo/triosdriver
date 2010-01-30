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

    public class WebServerImpl : WebServer
    {
        public WebServerImpl(string port): base ( port){
          
        } 

        protected override void handleRequest(HttpListenerContext context)
        {
            HttpListenerResponse res;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] r;
            string msg = "";

            msg = context.Request.HttpMethod +
                 System.Environment.NewLine +
                 context.Request.UserHostAddress +
                 System.Environment.NewLine +
                 context.Request.ContentLength64 +
                 System.Environment.NewLine +
                 context.Request.ContentEncoding;

            r = encoding.GetBytes(msg);
            res = context.Response;
            res.ContentLength64 = r.Length;
            res.OutputStream.Write(r, 0, r.Length);
            res.OutputStream.Close();
        }
    }
}
