
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
    public partial class TriosDriverGui : Form 
    {
        WebServerImpl web;

        public TriosDriverGui()
        {
            InitializeComponent();
        }


        private void bOpenSerialPort_Click(object sender, EventArgs e)
        {
            web = new WebServerImpl(textTCPPort.Text,textComPort.Text);
            web.Start();
           
        }
 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( web != null )
                web.Stop();
        }
    }
}
