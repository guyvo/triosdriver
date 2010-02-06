using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriosDriverForm
{
    interface ISerial
    {
        void Open(string port);
        void Send(byte[] data);
        void Close();
       
    }
}
