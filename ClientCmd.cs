using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using SocketTool.Core;
using System.Threading;

namespace SocketTool
{
    class ClientCmd
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ClientForm));
        private IClient socketClient = new CommTcpClient();
        private Thread SendOutgoingThread;

        private int sendInterval = 0;
        private Boolean IsAutoSend;

        private Boolean continueSend = false;

        private string sendContent;

        private string errorMsg = "";

        private string name = "";

        public SocketInfo SocketInfo { get; set; }

        public int fcounts = 0;

        public Boolean stopflag = false;

        public void SendData(object sender, EventArgs e)
        {
  
            socketClient.OnDataReceived += new ReceivedHandler(ListenMessage);
            socketClient.OnSocketError += new SocketErrorHandler(ListenErrorMessage);
            string ServerIP = this.SocketInfo.ServerIp;

            errorMsg = "";
            if (string.IsNullOrEmpty(ServerIP))
                errorMsg += "请输入合法的IP地址";
            try
            {
                int Port = this.SocketInfo.Port;
                socketClient.Init(ServerIP, Port);
            }
            catch (Exception ex)
            {
                errorMsg += "请输入合法的端口";
            }

            sendContent = this.SocketInfo.Data;
            
            if (string.IsNullOrEmpty(sendContent))
                errorMsg += "请输入要发送的内容";

                try
                {
                    sendInterval = int.Parse(this.SocketInfo.Interval) * 1000;
                }
                catch (Exception ex)
                {
                    errorMsg += "请输入整数的发送时间间隔";
                }

                IsAutoSend = true;
            
            SendOutgoingThread = new Thread(new ThreadStart(SendThreadFunc));
            SendOutgoingThread.Start();

        }
     
        private void SendThreadFunc()
        {
            continueSend = true;
            while (continueSend)
            {
                if (this.socketClient.Isconnect())
                   this.SocketInfo.Stopflag = false;

                byte[] data = System.Text.Encoding.Default.GetBytes(sendContent);
                data = ParseUtil.ToByesByHex(sendContent);
                try
                {
                    socketClient.Send(data);
                    this.SocketInfo.IsRefreshSend = true;
                    fcounts = 0;

                }
                catch (Exception ex)
                {
                    //deal with the error
                    break;
                }
                if (IsAutoSend == false)
                    break;
                Thread.Sleep(sendInterval);

            }
        }
        public void ListenMessage(object o, ReceivedEventArgs e)
        {
                
                try
                {
                    ReceivedHandler d = new ReceivedHandler(ListenMessage);
                    byte[] aa = e.Data;
                    this.SocketInfo.recData = aa;
                    this.SocketInfo.IsRefresh = true;
                    this.SocketInfo.Stopflag = false;
                    
                   
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }

        public void ListenErrorMessage(object o, SocketEventArgs e)
        {
            string errorMsg = "[" + e.ErrorCode + "]" + SocketUtil.DescrError(e.ErrorCode) + " -> "+ e.Message;
            this.SocketInfo.ErrorMsg = errorMsg;
            this.SocketInfo.IsRefreshError = true;

           //connect again
            byte[] data = System.Text.Encoding.Default.GetBytes(sendContent);
            data = ParseUtil.ToByesByHex(sendContent);
            this.SocketInfo.Stopflag = true;
            stopflag = false;

            if(!stopflag)
            {
                if (fcounts < 3)
                {
                    fcounts++;
                    Thread.Sleep(3000);
                    socketClient.Send(data);
                    this.SocketInfo.IsRefreshSend = true;

                }
                else if (fcounts < 10)
                {
                    Thread.Sleep(3000);
                    this.SocketInfo.ErrorMsg = "Close the Socket, and connect again! " + SocketInfo.ServerIp + ":" + SocketInfo.Port.ToString();
                    this.SocketInfo.IsRefreshError = true;
                    socketClient.Close();
                    socketClient.Send(data);
                    this.SocketInfo.IsRefreshSend = true;
                    fcounts++;

                }
                else
                    stopflag = true;

            }

            
            //if send data ok, then go out of here.
            //if test 3 times, please close the socket and connect agian.



        }
        
    }
}
