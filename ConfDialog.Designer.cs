namespace SocketTool
{
    partial class ConfDialog
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
            this.btSaveConfDialog = new System.Windows.Forms.Button();
            this.btCancelConfDialog = new System.Windows.Forms.Button();
            this.txtInitSerIp = new System.Windows.Forms.TextBox();
            this.txtInitPort = new System.Windows.Forms.TextBox();
            this.txtClientCounts = new System.Windows.Forms.TextBox();
            this.chkConnAll = new System.Windows.Forms.CheckBox();
            this.labInitSerIp = new System.Windows.Forms.Label();
            this.labInitPort = new System.Windows.Forms.Label();
            this.labCounts = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.labAddress = new System.Windows.Forms.Label();
            this.txtSendDelay = new System.Windows.Forms.TextBox();
            this.labInterval = new System.Windows.Forms.Label();
            this.chkDailyData = new System.Windows.Forms.CheckBox();
            this.chkMeterAlarm = new System.Windows.Forms.CheckBox();
            this.chkLoadProfile = new System.Windows.Forms.CheckBox();
            this.txtMeterNum = new System.Windows.Forms.TextBox();
            this.txtLoadInterval = new System.Windows.Forms.TextBox();
            this.labMeterNum = new System.Windows.Forms.Label();
            this.labLoadInterval = new System.Windows.Forms.Label();
            this.chkGui = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btSaveConfDialog
            // 
            this.btSaveConfDialog.Location = new System.Drawing.Point(15, 259);
            this.btSaveConfDialog.Name = "btSaveConfDialog";
            this.btSaveConfDialog.Size = new System.Drawing.Size(245, 34);
            this.btSaveConfDialog.TabIndex = 0;
            this.btSaveConfDialog.Text = "OK";
            this.btSaveConfDialog.UseVisualStyleBackColor = true;
            this.btSaveConfDialog.Click += new System.EventHandler(this.btSaveConfDialog_Click);
            // 
            // btCancelConfDialog
            // 
            this.btCancelConfDialog.Location = new System.Drawing.Point(283, 259);
            this.btCancelConfDialog.Name = "btCancelConfDialog";
            this.btCancelConfDialog.Size = new System.Drawing.Size(263, 34);
            this.btCancelConfDialog.TabIndex = 1;
            this.btCancelConfDialog.Text = "CANCEL";
            this.btCancelConfDialog.UseVisualStyleBackColor = true;
            this.btCancelConfDialog.Click += new System.EventHandler(this.btCancelConfDialog_Click);
            // 
            // txtInitSerIp
            // 
            this.txtInitSerIp.Location = new System.Drawing.Point(104, 39);
            this.txtInitSerIp.Name = "txtInitSerIp";
            this.txtInitSerIp.Size = new System.Drawing.Size(135, 25);
            this.txtInitSerIp.TabIndex = 2;
            this.txtInitSerIp.Text = "localhost";
            // 
            // txtInitPort
            // 
            this.txtInitPort.Location = new System.Drawing.Point(333, 39);
            this.txtInitPort.Name = "txtInitPort";
            this.txtInitPort.Size = new System.Drawing.Size(120, 25);
            this.txtInitPort.TabIndex = 3;
            this.txtInitPort.Text = "90";
            // 
            // txtClientCounts
            // 
            this.txtClientCounts.Location = new System.Drawing.Point(139, 97);
            this.txtClientCounts.Name = "txtClientCounts";
            this.txtClientCounts.Size = new System.Drawing.Size(100, 25);
            this.txtClientCounts.TabIndex = 4;
            this.txtClientCounts.Text = "20";
            // 
            // chkConnAll
            // 
            this.chkConnAll.AutoSize = true;
            this.chkConnAll.Location = new System.Drawing.Point(344, 106);
            this.chkConnAll.Name = "chkConnAll";
            this.chkConnAll.Size = new System.Drawing.Size(109, 19);
            this.chkConnAll.TabIndex = 5;
            this.chkConnAll.Text = "ConnectAll";
            this.chkConnAll.UseVisualStyleBackColor = true;
            // 
            // labInitSerIp
            // 
            this.labInitSerIp.AutoSize = true;
            this.labInitSerIp.Location = new System.Drawing.Point(19, 49);
            this.labInitSerIp.Name = "labInitSerIp";
            this.labInitSerIp.Size = new System.Drawing.Size(79, 15);
            this.labInitSerIp.TabIndex = 6;
            this.labInitSerIp.Text = "ServerIp:";
            // 
            // labInitPort
            // 
            this.labInitPort.AutoSize = true;
            this.labInitPort.Location = new System.Drawing.Point(280, 49);
            this.labInitPort.Name = "labInitPort";
            this.labInitPort.Size = new System.Drawing.Size(47, 15);
            this.labInitPort.TabIndex = 7;
            this.labInitPort.Text = "Port:";
            // 
            // labCounts
            // 
            this.labCounts.AutoSize = true;
            this.labCounts.Location = new System.Drawing.Point(19, 107);
            this.labCounts.Name = "labCounts";
            this.labCounts.Size = new System.Drawing.Size(111, 15);
            this.labCounts.TabIndex = 8;
            this.labCounts.Text = "ClientCounts:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(229, 153);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(197, 25);
            this.txtAddress.TabIndex = 9;
            this.txtAddress.Text = "28020001";
            // 
            // labAddress
            // 
            this.labAddress.AutoSize = true;
            this.labAddress.Location = new System.Drawing.Point(12, 156);
            this.labAddress.Name = "labAddress";
            this.labAddress.Size = new System.Drawing.Size(199, 15);
            this.labAddress.TabIndex = 10;
            this.labAddress.Text = "Teminal Initial Address:";
            // 
            // txtSendDelay
            // 
            this.txtSendDelay.Location = new System.Drawing.Point(229, 197);
            this.txtSendDelay.Name = "txtSendDelay";
            this.txtSendDelay.Size = new System.Drawing.Size(100, 25);
            this.txtSendDelay.TabIndex = 11;
            this.txtSendDelay.Text = "120";
            // 
            // labInterval
            // 
            this.labInterval.AutoSize = true;
            this.labInterval.Location = new System.Drawing.Point(100, 197);
            this.labInterval.Name = "labInterval";
            this.labInterval.Size = new System.Drawing.Size(111, 15);
            this.labInterval.TabIndex = 12;
            this.labInterval.Text = "Send Interval";
            // 
            // chkDailyData
            // 
            this.chkDailyData.AutoSize = true;
            this.chkDailyData.Location = new System.Drawing.Point(551, 61);
            this.chkDailyData.Name = "chkDailyData";
            this.chkDailyData.Size = new System.Drawing.Size(165, 19);
            this.chkDailyData.TabIndex = 13;
            this.chkDailyData.Text = "Daily Frozen Data";
            this.chkDailyData.UseVisualStyleBackColor = true;
            // 
            // chkMeterAlarm
            // 
            this.chkMeterAlarm.AutoSize = true;
            this.chkMeterAlarm.Location = new System.Drawing.Point(551, 107);
            this.chkMeterAlarm.Name = "chkMeterAlarm";
            this.chkMeterAlarm.Size = new System.Drawing.Size(173, 19);
            this.chkMeterAlarm.TabIndex = 14;
            this.chkMeterAlarm.Text = "Meter Alarm Events";
            this.chkMeterAlarm.UseVisualStyleBackColor = true;
            // 
            // chkLoadProfile
            // 
            this.chkLoadProfile.AutoSize = true;
            this.chkLoadProfile.Location = new System.Drawing.Point(551, 155);
            this.chkLoadProfile.Name = "chkLoadProfile";
            this.chkLoadProfile.Size = new System.Drawing.Size(125, 19);
            this.chkLoadProfile.TabIndex = 15;
            this.chkLoadProfile.Text = "Load Profile";
            this.chkLoadProfile.UseVisualStyleBackColor = true;
            // 
            // txtMeterNum
            // 
            this.txtMeterNum.Location = new System.Drawing.Point(672, 197);
            this.txtMeterNum.Name = "txtMeterNum";
            this.txtMeterNum.Size = new System.Drawing.Size(100, 25);
            this.txtMeterNum.TabIndex = 16;
            this.txtMeterNum.Text = "5";
            // 
            // txtLoadInterval
            // 
            this.txtLoadInterval.Location = new System.Drawing.Point(672, 245);
            this.txtLoadInterval.Name = "txtLoadInterval";
            this.txtLoadInterval.Size = new System.Drawing.Size(100, 25);
            this.txtLoadInterval.TabIndex = 17;
            this.txtLoadInterval.Text = "5";
            // 
            // labMeterNum
            // 
            this.labMeterNum.AutoSize = true;
            this.labMeterNum.Location = new System.Drawing.Point(552, 207);
            this.labMeterNum.Name = "labMeterNum";
            this.labMeterNum.Size = new System.Drawing.Size(111, 15);
            this.labMeterNum.TabIndex = 18;
            this.labMeterNum.Text = "Meter numbers";
            // 
            // labLoadInterval
            // 
            this.labLoadInterval.AutoSize = true;
            this.labLoadInterval.Location = new System.Drawing.Point(552, 248);
            this.labLoadInterval.Name = "labLoadInterval";
            this.labLoadInterval.Size = new System.Drawing.Size(111, 15);
            this.labLoadInterval.TabIndex = 19;
            this.labLoadInterval.Text = "Time Interval";
            // 
            // chkGui
            // 
            this.chkGui.AutoSize = true;
            this.chkGui.Location = new System.Drawing.Point(274, 106);
            this.chkGui.Name = "chkGui";
            this.chkGui.Size = new System.Drawing.Size(53, 19);
            this.chkGui.TabIndex = 20;
            this.chkGui.Text = "GUI";
            this.chkGui.UseVisualStyleBackColor = true;
            // 
            // ConfDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 319);
            this.Controls.Add(this.chkGui);
            this.Controls.Add(this.labLoadInterval);
            this.Controls.Add(this.labMeterNum);
            this.Controls.Add(this.txtLoadInterval);
            this.Controls.Add(this.txtMeterNum);
            this.Controls.Add(this.chkLoadProfile);
            this.Controls.Add(this.chkMeterAlarm);
            this.Controls.Add(this.chkDailyData);
            this.Controls.Add(this.labInterval);
            this.Controls.Add(this.txtSendDelay);
            this.Controls.Add(this.labAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.labCounts);
            this.Controls.Add(this.labInitPort);
            this.Controls.Add(this.labInitSerIp);
            this.Controls.Add(this.chkConnAll);
            this.Controls.Add(this.txtClientCounts);
            this.Controls.Add(this.txtInitPort);
            this.Controls.Add(this.txtInitSerIp);
            this.Controls.Add(this.btCancelConfDialog);
            this.Controls.Add(this.btSaveConfDialog);
            this.Name = "ConfDialog";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.ConfDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSaveConfDialog;
        private System.Windows.Forms.Button btCancelConfDialog;
        private System.Windows.Forms.TextBox txtInitSerIp;
        private System.Windows.Forms.TextBox txtInitPort;
        private System.Windows.Forms.TextBox txtClientCounts;
        private System.Windows.Forms.CheckBox chkConnAll;
        private System.Windows.Forms.Label labInitSerIp;
        private System.Windows.Forms.Label labInitPort;
        private System.Windows.Forms.Label labCounts;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label labAddress;
        private System.Windows.Forms.TextBox txtSendDelay;
        private System.Windows.Forms.Label labInterval;
        private System.Windows.Forms.CheckBox chkDailyData;
        private System.Windows.Forms.CheckBox chkMeterAlarm;
        private System.Windows.Forms.CheckBox chkLoadProfile;
        private System.Windows.Forms.TextBox txtMeterNum;
        private System.Windows.Forms.TextBox txtLoadInterval;
        private System.Windows.Forms.Label labMeterNum;
        private System.Windows.Forms.Label labLoadInterval;
        private System.Windows.Forms.CheckBox chkGui;
    }
}