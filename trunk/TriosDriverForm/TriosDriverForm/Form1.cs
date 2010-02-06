
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
        WebServerImpl web;

        public Form1()
        {
            InitializeComponent();
        }


        private void bOpenSerialPort_Click(object sender, EventArgs e)
        {
            web = new WebServerImpl("8080",textComPort.Text);
            web.Start();
           
        }
 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            web.Stop();
        }
    
    }
}
