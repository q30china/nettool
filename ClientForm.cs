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
    public partial class ClientForm : Form, ISocketInfo
    {
        public ClientForm()
        {
            InitializeComponent();
            SocketInfo = new SocketInfo();

        }

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ClientForm));  
        private IClient socketClient = new CommTcpClient();
        private Thread SendOutgoingThread ;
        private Thread RefreshRecvThread;

        private int sendInterval = 0;
        private Boolean IsAutoSend;

        private Boolean continueSend = false;

        private string sendContent;

        private string errorMsg = "";

        public SocketInfo SocketInfo {get;set;}

        private delegate void myDelegate();

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (rbUdp.Checked) 
                socketClient = new CommUdpClient();

            socketClient.OnDataReceived += new ReceivedHandler(ListenMessage);
            socketClient.OnSocketError += new SocketErrorHandler(ListenErrorMessage);
            string ServerIP = this.txtIP.Text;
            //string ServerIP = this.SocketInfo.ServerIp;
  
            errorMsg = "";
            if (string.IsNullOrEmpty(ServerIP))
                errorMsg += "请输入合法的IP地址";
            try
            {
                int Port = int.Parse(this.txtPort.Text);
                //int Port = this.SocketInfo.Port;
                socketClient.Init(ServerIP, Port);
            }
            catch (Exception ex)
            {
                errorMsg += "请输入合法的端口";
            }

            sendContent = this.rtSendData.Text;
            if (string.IsNullOrEmpty(sendContent))
                errorMsg += "请输入要发送的内容";
            
            if (cbAutoSend.Checked)
            {
                try
                {
                    sendInterval = int.Parse(txtInterval.Text) * 1000;
                }
                catch (Exception ex)
                {
                    errorMsg += "请输入整数的发送时间间隔";
                }
                
                IsAutoSend = true;
            }
            if (string.IsNullOrEmpty(errorMsg) == false)
            {
                MessageBox.Show(errorMsg);
                return;
            }
            continueSend = true;
            btnDisconnect.Enabled = true;
            btnSend.Enabled = IsAutoSend == false;
            SendOutgoingThread = new Thread(new ThreadStart(SendThreadFunc));
            SendOutgoingThread.Start();
            /**
            else
            {
                if (string.IsNullOrEmpty(errorMsg) == false)
                {
                    MessageBox.Show(errorMsg);
                    return;
                }
                byte[] data = System.Text.Encoding.Default.GetBytes(sendContent);
                try
                {
                    tcpClient.SendData(data);
                    btnDisconnect.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送数据出错，无法连接服务器");
                }
            }
             */
            
        }

        private void btnSend_Clickorg(object sender, EventArgs e)
        {
            if (rbUdp.Checked)
                socketClient = new CommUdpClient();

            socketClient.OnDataReceived += new ReceivedHandler(ListenMessage);
            socketClient.OnSocketError += new SocketErrorHandler(ListenErrorMessage);
            //string ServerIP = this.txtIP.Text;
            string ServerIP = this.SocketInfo.ServerIp;


            errorMsg = "";
            if (string.IsNullOrEmpty(ServerIP))
                errorMsg += "请输入合法的IP地址";
            try
            {
                //int Port = int.Parse(this.txtPort.Text);
                int Port = this.SocketInfo.Port;
                socketClient.Init(ServerIP, Port);
            }
            catch (Exception ex)
            {
                errorMsg += "请输入合法的端口";
            }

            sendContent = this.rtSendData.Text;
            if (string.IsNullOrEmpty(sendContent))
                errorMsg += "请输入要发送的内容";

            if (cbAutoSend.Checked)
            {
                try
                {
                    sendInterval = int.Parse(txtInterval.Text) * 1000;
                }
                catch (Exception ex)
                {
                    errorMsg += "请输入整数的发送时间间隔";
                }

                IsAutoSend = true;
            }
            if (string.IsNullOrEmpty(errorMsg) == false)
            {
                MessageBox.Show(errorMsg);
                return;
            }
            continueSend = true;
            btnDisconnect.Enabled = true;
            btnSend.Enabled = IsAutoSend == false;
            SendOutgoingThread = new Thread(new ThreadStart(SendThreadFunc));
            SendOutgoingThread.Start();
            /**
            else
            {
                if (string.IsNullOrEmpty(errorMsg) == false)
                {
                    MessageBox.Show(errorMsg);
                    return;
                }
                byte[] data = System.Text.Encoding.Default.GetBytes(sendContent);
                try
                {
                    tcpClient.SendData(data);
                    btnDisconnect.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送数据出错，无法连接服务器");
                }
            }
             */

        }
        private void SendThreadFunc()
        {
            while (continueSend)
            {
                byte[] data = System.Text.Encoding.Default.GetBytes(sendContent);

                if(rbHex.Checked)
                {
                    data = ParseUtil.ToByesByHex(sendContent);
                }

                try
                {
                    socketClient.Send(data);
                }
                catch (Exception ex)
                {
                    ListenMessage(0, "", ex.Message);
                    break;
                }
                if (IsAutoSend == false)
                    break;
                Thread.Sleep(sendInterval); 
   
            }
        }
        public void ListenErrorMessage(object o, SocketEventArgs e)
        {
            string errorMsg = "[" + e.ErrorCode + "]" + SocketUtil.DescrError(e.ErrorCode);

            ListenMessage((int)o, "Socket错误", errorMsg);

        }

        private void ListenMessage(object ID, string type, string msg)
        {
            if (PacketView.InvokeRequired)
            {
                try
                {
                    MsgHandler d = new MsgHandler(ListenMessage);
                    this.Invoke(d, new object[] {0,type, msg});
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
            else
            {
                if (type == "Socket错误")
                {
                    continueSend = false;
                    btnDisconnect.Enabled = false;
                    btnSend.Enabled = true;
                }
                if (PacketView.Items.Count > 200)
                    PacketView.Items.Clear();


                ListViewItem item = PacketView.Items.Insert(0, "" + PacketView.Items.Count);

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
                try
                {
                    ReceivedHandler d = new ReceivedHandler(ListenMessage);
                    this.Invoke(d, new object[] {o, e });
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

                int length = e.Data.Length;
                string strDate = DateTime.Now.ToString("HH:mm:ss");
                item.SubItems.Add(strDate);
                string msg = ParseUtil.ParseString(e.Data, length);
                if (rbHex.Checked)
                    msg = ParseUtil.ToHexString(e.Data, length);
                item.SubItems.Add(msg);
                item.SubItems.Add("" + length);

                if (cbLog.Checked)
                {
                    logger.Info(e.RemoteHost.ToString() + " " + msg);
                }
                //item.SubItems.Add("" + msg.MsgContentDesc);
            }
        }

        private void cbAutoSend_CheckedChanged(object sender, EventArgs e)
        {
            this.txtInterval.Enabled = cbAutoSend.Checked;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            continueSend = false;
            try
            {
                if (socketClient != null)
                    socketClient.Close();
                SendOutgoingThread.Abort();
            }
            catch (Exception ex)
            {
            }

            this.btnSend.Enabled = true;
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
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
            SocketInfo.Type = "Client";
            SocketInfo.ServerIp = this.txtIP.Text;
            SocketInfo.Data = this.rtSendData.Text;

            SocketInfo.IsAuto = cbAutoSend.Checked;
            try
            {
                SocketInfo.Port = int.Parse(this.txtPort.Text);
            }
            catch (Exception ex)
            {
                //errorMsg += "请输入合法的端口";
            }
            

        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            this.PacketView.Clear();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            this.txtIP.Text = SocketInfo.ServerIp;
            rbTcp.Checked = SocketInfo.Protocol == "Tcp";
            rbUdp.Checked = SocketInfo.Protocol != "Tcp";
            rbAscII.Checked = SocketInfo.Format == "AscII";
            rbHex.Checked = SocketInfo.Format != "AscII";
            this.txtPort.Text = "" + SocketInfo.Port;
            this.rtSendData.Text = SocketInfo.Data;
            this.txtInterval.Text = SocketInfo.Interval;
            this.cbAutoSend.Checked = SocketInfo.IsAuto;
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "client.log");
        }

        public void btnAutoSend()
        {
            btnSend_Click(null,null);
        }

       // public void RefreshData(object sender, EventArgs e)
        public void RefreshData()
        {
            RefreshRecvThread = new Thread(new ThreadStart(RefreshDataFun));
            RefreshRecvThread.Start();
            
            this.txtIP.Enabled = false;
            this.txtPort.Enabled = false;
            this.txtInterval.Enabled = false;
            this.cbAutoSend.Checked = this.SocketInfo.IsAuto;
            this.cbAutoSend.Enabled = false;
            this.btnSend.Enabled = false;
            //this.btnDisconnect.Enabled = false;
            this.rbAscII.Enabled = false;
            this.rbHex.Enabled = false;
            this.rbTcp.Enabled = false;
            this.rbUdp.Enabled = false;
  
        }

        private void RefreshDataFun1()
        {

            if (this.InvokeRequired)
              {
                  this.Invoke(new myDelegate(RefreshDataFun1));
              }
             else
             {
                 if (this.SocketInfo.IsRefreshError)
                 {
                     ListViewItem item = PacketView.Items.Insert(0, "" + PacketView.Items.Count);
                     //int length = e.Data.Length;
                     string strDate = DateTime.Now.ToString("HH:mm:ss");
                     item.SubItems.Add("Receve: ");
                     item.SubItems.Add(strDate);
                     item.SubItems.Add(this.SocketInfo.ErrorMsg);
                     this.SocketInfo.IsRefreshError = false;
                 }
            
                 if(this.SocketInfo.IsRefreshSend)
                 {
                     if (PacketView.Items.Count > 200)
                         PacketView.Items.Clear();

                     ListViewItem item = PacketView.Items.Insert(0, " " + PacketView.Items.Count);
                     item.ForeColor = Color.Blue;

                     string str1 = this.SocketInfo.Data.Replace(" ","");
                     int length = str1.Length / 2;

                     string msg = this.SocketInfo.Data;
                     //if (rbHex.Checked)
                     //    msg = ParseUtil.ToHexString(this.SocketInfo.Data, length);

                     string strDate = DateTime.Now.ToString("HH:mm:ss");
                     item.SubItems.Add("Send:");
                     item.SubItems.Add(strDate);
                     item.SubItems.Add(msg);
                     item.SubItems.Add("" + length);
                     this.SocketInfo.IsRefreshSend = false;
                 }

                 if (this.SocketInfo.IsRefresh)
                 {

                     if (PacketView.Items.Count > 200)
                         PacketView.Items.Clear();

                     ListViewItem item = PacketView.Items.Insert(0, " " + PacketView.Items.Count);
                     item.ForeColor = Color.Red;
                     //item.SubItems.Add("Recevie:");

                     int length = this.SocketInfo.recData.Length;

                     string msg = ParseUtil.ParseString(this.SocketInfo.recData, length);
                     if (rbHex.Checked)
                          msg = ParseUtil.ToHexString(this.SocketInfo.recData, length);

                     string strDate = DateTime.Now.ToString("HH:mm:ss");
                     item.SubItems.Add("Receive:");
                     item.SubItems.Add(strDate);
                     //转换下数据报文格式
                     string str = Util.AddSpaceToFrame(msg);
                     item.SubItems.Add(str);
                     item.SubItems.Add("" + length);

                     //if (cbLog.Checked)
                     //{
                     //   logger.Info(e.RemoteHost.ToString() + " " + msg);
                     //}
                     this.SocketInfo.IsRefresh = false;
                 }
             }
        }

         private void RefreshDataFun()
         {
            while (true)
            {
               RefreshDataFun1();
               Thread.Sleep(100); 
             }
             
          }

         private void PacketView_MouseDoubleClick(object sender, MouseEventArgs e)
         {
             string msg1 = "";
             if (this.SocketInfo.Stopflag == false)
                 msg1 = msg1 + " Connected!";
             else
                 msg1 = msg1 + "Disconneted!";

             string aa = "--->" + this.SocketInfo.ServerIp + " : " + this.SocketInfo.Port;
             MessageBox.Show("Send:" + this.SocketInfo.Data + "\n" + " Socket ->" + msg1 , this.SocketInfo.Name + " " + aa, MessageBoxButtons.OK);

             int length = this.SocketInfo.recData.Length;

             string msg = ParseUtil.ParseString(this.SocketInfo.recData, length);
             if (rbHex.Checked)
                 msg = ParseUtil.ToHexString(this.SocketInfo.recData, length);


             MessageBox.Show(msg, this.SocketInfo.Name, MessageBoxButtons.OK);

         }
        
        
    }
}
