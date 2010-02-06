using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace TriosDriverForm
{
    abstract class SerialController
    {
        protected const int maxBUFFER = 4096;

        protected SerialPort serialPort;
        protected ManualResetEvent manualEvent = new ManualResetEvent(false);
        protected byte[] buffer = new byte[maxBUFFER];

        private int idx = 0;
        private bool _isOpen = false;

        public SerialController( string port )
        {
            serialPort = new SerialPort( port );
            serialPort.BaudRate = 4500000;
            serialPort.WriteBufferSize = maxBUFFER;
            serialPort.ReadBufferSize = maxBUFFER;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            serialPort.Open();
        }

        public void SendData(byte[] data)
        {
            if (data.Length <= maxBUFFER)
            {
                serialPort.Write(data, 0, data.Length);
                manualEvent.WaitOne();
                idx = 0;
                ProcessData();
                manualEvent.Reset();
            }
        }

        public bool isOpen
        {
            get
            {
                return _isOpen;
            }

        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int temp;

            temp = serialPort.Read(buffer, idx, buffer.Length - idx);
            idx += temp;
            if (idx == buffer.Length)
                manualEvent.Set();
        }
    
        protected abstract void ProcessData ( );

        public int isReceived
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        
    }
}
