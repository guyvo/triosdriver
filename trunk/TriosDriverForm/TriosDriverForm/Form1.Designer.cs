namespace TriosDriverForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.bOpenSerialPort = new System.Windows.Forms.Button();
            this.rIsConnected = new System.Windows.Forms.RadioButton();
            this.bSendBuffer = new System.Windows.Forms.Button();
            this.textStatus = new System.Windows.Forms.TextBox();
            this.bReadBytes = new System.Windows.Forms.Button();
            this.textComPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 4500000;
            this.serialPort1.PortName = "COM3";
            this.serialPort1.WriteBufferSize = 4096;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // bOpenSerialPort
            // 
            this.bOpenSerialPort.Location = new System.Drawing.Point(26, 35);
            this.bOpenSerialPort.Name = "bOpenSerialPort";
            this.bOpenSerialPort.Size = new System.Drawing.Size(185, 23);
            this.bOpenSerialPort.TabIndex = 0;
            this.bOpenSerialPort.Text = "Open serial port";
            this.bOpenSerialPort.UseVisualStyleBackColor = true;
            this.bOpenSerialPort.Click += new System.EventHandler(this.bOpenSerialPort_Click);
            // 
            // rIsConnected
            // 
            this.rIsConnected.AutoSize = true;
            this.rIsConnected.Location = new System.Drawing.Point(254, 40);
            this.rIsConnected.Name = "rIsConnected";
            this.rIsConnected.Size = new System.Drawing.Size(77, 17);
            this.rIsConnected.TabIndex = 1;
            this.rIsConnected.TabStop = true;
            this.rIsConnected.Text = "Connected";
            this.rIsConnected.UseVisualStyleBackColor = true;
            // 
            // bSendBuffer
            // 
            this.bSendBuffer.Enabled = false;
            this.bSendBuffer.Location = new System.Drawing.Point(26, 106);
            this.bSendBuffer.Name = "bSendBuffer";
            this.bSendBuffer.Size = new System.Drawing.Size(185, 23);
            this.bSendBuffer.TabIndex = 2;
            this.bSendBuffer.Text = "Send bytes";
            this.bSendBuffer.UseVisualStyleBackColor = true;
            this.bSendBuffer.Click += new System.EventHandler(this.bSendBuffer_Click);
            // 
            // textStatus
            // 
            this.textStatus.Location = new System.Drawing.Point(254, 181);
            this.textStatus.Multiline = true;
            this.textStatus.Name = "textStatus";
            this.textStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textStatus.Size = new System.Drawing.Size(492, 142);
            this.textStatus.TabIndex = 3;
            // 
            // bReadBytes
            // 
            this.bReadBytes.Location = new System.Drawing.Point(26, 181);
            this.bReadBytes.Name = "bReadBytes";
            this.bReadBytes.Size = new System.Drawing.Size(185, 23);
            this.bReadBytes.TabIndex = 4;
            this.bReadBytes.Text = "Read the bytes";
            this.bReadBytes.UseVisualStyleBackColor = true;
            this.bReadBytes.Click += new System.EventHandler(this.bReadBytes_Click);
            // 
            // textComPort
            // 
            this.textComPort.Location = new System.Drawing.Point(254, 63);
            this.textComPort.Name = "textComPort";
            this.textComPort.Size = new System.Drawing.Size(49, 20);
            this.textComPort.TabIndex = 5;
            this.textComPort.Text = "COM3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 348);
            this.Controls.Add(this.textComPort);
            this.Controls.Add(this.bReadBytes);
            this.Controls.Add(this.textStatus);
            this.Controls.Add(this.bSendBuffer);
            this.Controls.Add(this.rIsConnected);
            this.Controls.Add(this.bOpenSerialPort);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button bOpenSerialPort;
        private System.Windows.Forms.RadioButton rIsConnected;
        private System.Windows.Forms.Button bSendBuffer;
        private System.Windows.Forms.TextBox textStatus;
        private System.Windows.Forms.Button bReadBytes;
        private System.Windows.Forms.TextBox textComPort;
    }
}

