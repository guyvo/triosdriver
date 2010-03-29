namespace TriosDriverForm
{
    partial class TriosDriverGui
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
            this.bOpenSerialPort = new System.Windows.Forms.Button();
            this.textComPort = new System.Windows.Forms.TextBox();
            this.textTCPPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bOpenSerialPort
            // 
            this.bOpenSerialPort.Location = new System.Drawing.Point(23, 35);
            this.bOpenSerialPort.Name = "bOpenSerialPort";
            this.bOpenSerialPort.Size = new System.Drawing.Size(107, 23);
            this.bOpenSerialPort.TabIndex = 0;
            this.bOpenSerialPort.Text = "Start server";
            this.bOpenSerialPort.UseVisualStyleBackColor = true;
            this.bOpenSerialPort.Click += new System.EventHandler(this.bOpenSerialPort_Click);
            // 
            // textComPort
            // 
            this.textComPort.Location = new System.Drawing.Point(148, 38);
            this.textComPort.Name = "textComPort";
            this.textComPort.Size = new System.Drawing.Size(39, 20);
            this.textComPort.TabIndex = 5;
            this.textComPort.Text = "COM5";
            this.textComPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textTCPPort
            // 
            this.textTCPPort.Location = new System.Drawing.Point(149, 73);
            this.textTCPPort.Name = "textTCPPort";
            this.textTCPPort.Size = new System.Drawing.Size(38, 20);
            this.textTCPPort.TabIndex = 6;
            this.textTCPPort.Text = "60000";
            this.textTCPPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TriosDriverGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 161);
            this.Controls.Add(this.textTCPPort);
            this.Controls.Add(this.textComPort);
            this.Controls.Add(this.bOpenSerialPort);
            this.Name = "TriosDriverGui";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOpenSerialPort;
        private System.Windows.Forms.TextBox textComPort;
        private System.Windows.Forms.TextBox textTCPPort;
    }
}

