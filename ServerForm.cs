﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SocketTool.Core;
using System.Threading;
using System.Diagnostics;

namespace SocketTool
{
    public partial class ServerForm : Form, ISocketInfo
    {
        public ServerForm()
        {
            InitializeComponent();
            SocketInfo = new SocketInfo();

        }

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ServerForm));  
        private IServer commServer = new CommTcpServer();

        private Thread refreshThread;

        private Boolean continueRefresh = true;

        private List<IConnection> conns = new List<IConnection>();

        public SocketInfo SocketInfo { get; set; }

        private void btnSend_Click(object sender, EventArgs e)
        {
        }

        private void btnListen_Click(object sender, EventArgs e)
        {

            if (rbUdp.Checked)
                commServer = new CommUdpServer();
            int port = int.Parse(this.txtPort.Text);

            commServer.Init(null, port);
            commServer.OnDataReceived += new ReceivedHandler(ListenMessage);
            commServer.OnSocketError += new SocketErrorHandler(ListenErrorMessage);

            //refreshThread = new Thread(new ThreadStart(RefreshConnection));
            if(refreshConnectionWorker.IsBusy == false)
               refreshConnectionWorker.RunWorkerAsync();
            try
            {
                commServer.Listen();
                btnListen.Enabled = false;
                btnStopListen.Enabled = true;
                ListenMessage(0, "", "启动监听成功,端口:" + port);
            }
            catch (Exception ex)
            {
                MessageBox.Show("监听失败，端口可能被占用");
            }
        }

        public void ListenErrorMessage(object o, SocketEventArgs e)
        {
            string errorMsg = "[" + e.ErrorCode + "]" + SocketUtil.DescrError(e.ErrorCode);

            ListenMessage(o, "Socket错误", errorMsg);

        }
        private void ListenMessage(object ID, string type, string msg)
        {
            if (PacketView.InvokeRequired)
            {
                try
                {
                    MsgHandler d = new MsgHandler(ListenMessage);
                    this.Invoke(d, new object[] { ID,type, msg });
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
            else
            {
                if (PacketView.Items.Count > 200)
                    PacketView.Items.Clear();


                ListViewItem item = PacketView.Items.Insert(0, "" + PacketView.Items.Count);

                item.SubItems.Add("" + ID);
                item.SubItems.Add("");
                //int length = e.Data.Length;
                string strDate = DateTime.Now.ToString("HH:mm:ss");
                item.SubItems.Add(strDate);
                item.SubItems.Add(msg);
                //item.SubItems.Add("" + length);
            }
        }

        public void ListenMessage(object o, ReceivedEventArgs e)
        {
            if (PacketView.InvokeRequired)
            {
                
                    ReceivedHandler d = new ReceivedHandler(ListenMessage);
                    this.Invoke(d, new object[] { o, e});
            }
            else
            {
                byte[] data = e.Data;
                int length = data.Length;
                Boolean isEcho = this.cbAutoSend.Checked;
                string connId = "" + o;
                if (isEcho)
                {
                    commServer.Send(connId, data, data.Length);
                }

                
                if (PacketView.Items.Count > 200)
                    PacketView.Items.Clear();

                ListViewItem item = PacketView.Items.Insert(0, "" + PacketView.Items.Count);
                item.SubItems.Add("" + connId);
                item.SubItems.Add("" + e.RemoteHost.ToString());

                string msg = ParseUtil.ParseString(data, length);
                if (rbHex.Checked)
                {
                    msg = ParseUtil.ToHexString(data, length);
                }

                string strDate = DateTime.Now.ToString("HH:mm:ss");
                item.SubItems.Add(strDate);

                item.SubItems.Add(msg);
                item.SubItems.Add("" + length);
                if (cbLog.Checked)
                {
                    logger.Info(e.RemoteHost.ToString() + " " + msg);
                }
                //item.SubItems.Add("" + msg.MsgContentDesc);
            }
        }

        private void btnStopListen_Click(object sender, EventArgs e)
        {
            commServer.Close();

            btnListen.Enabled = true;
            btnStopListen.Enabled = false;
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            SocketInfo.ServerIp = this.txtIP.Text;
            try
            {
            	SocketInfo.Port = int.Parse(this.txtPort.Text);
            }
            catch (System.Exception ex)
            {
            	
            }
            SocketInfo.Protocol = rbTcp.Checked ? "Tcp" : "Udp";
            SocketInfo.Format = rbAscII.Checked ? "AscII" : "Hex";
            SocketInfo.Type = "Server";
            SocketInfo.Data = this.rtbData.Text;
            SocketInfo.IsAuto = cbAutoSend.Checked;
            refreshConnectionWorker.CancelAsync();
            commServer.Close();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            this.PacketView.Items.Clear();
        }

        private void refreshConnectionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
             int m = 0;
             while (continueRefresh)
             {
                 conns = commServer.GetConnectionList();
                 refreshConnectionWorker.ReportProgress(m);

                 Thread.Sleep(1000);
             }
        }

        private void refreshConnectionWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {

                connectionView.Items.Clear();
                foreach (IConnection ic in conns)
                {

                    ListViewItem item = connectionView.Items.Insert(0, "" + connectionView.Items.Count);
                    item.Tag = ic.ID;
                    item.SubItems.Add("" + ic.ID);
                    //int length = e.Data.Length;
                    string strDate = DateTime.Now.ToString("dd HH:mm:ss");
                    item.SubItems.Add(ic.CreateDate.ToString("dd HH:mm:ss"));
                    item.SubItems.Add(ic.ClientIP.ToString());
                    item.SubItems.Add(ic.OnlineDate.ToString("dd HH:mm:ss"));

                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private string selectedConnectionID;
        private void connectionView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in connectionView.SelectedItems)
            {
                // selectedConnectionID = ""+item.Tag;

                txtConn.Text = item.SubItems[1].Text;
            }
            
        }

        private void PacketView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PacketView.SelectedItems)
            {
                selectedConnectionID = ""+item.Tag;
                rtbData.Text = item.SubItems[4].Text;
            }
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            this.txtIP.Text = SocketInfo.ServerIp;
            rbTcp.Checked = SocketInfo.Protocol == "Tcp";
            rbUdp.Checked = SocketInfo.Protocol != "Tcp";
            rbAscII.Checked = SocketInfo.Format == "AscII";
            rbHex.Checked = SocketInfo.Format != "AscII";
            this.txtPort.Text = "" + SocketInfo.Port;

            cbAutoSend.Checked = SocketInfo.IsAuto;
            this.rtbData.Text = SocketInfo.Data;
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "server.log");
        }

        /// <summary>
        /// 向特定的Sokcet发送数据
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click_1(object sender, EventArgs e)
        {
            //当前连接 txtConn
            //发送数据 richTextBox2
            byte[] data = System.Text.Encoding.Default.GetBytes(richTextBox2.Text);
            if (rbHex.Checked)
            {
                data = ParseUtil.ToByesByHex(richTextBox2.Text);
            }

            commServer.Send(txtConn.Text, data, data.Length);
        }

        /// <summary>
        /// 断掉选中的Socket连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            //
        }

    }
}
