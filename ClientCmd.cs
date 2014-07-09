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
        private Thread GetSenddataThread;

        private int sendInterval = 0;
        private Boolean IsAutoSend;

        private Boolean continueSend = false;

        private string sendContent;

        private string errorMsg = "";

        private string name = "";

        public SocketInfo SocketInfo { get; set; }

        private List<string> datalist = new List<string>();

        public int fcounts = 0;

        public Boolean stopflag = false;

        private Boolean firstflag = true;
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

            GetSenddataThread = new Thread(new ThreadStart(GetDataThreadFunc));
            GetSenddataThread.Start();

        }
     
        private void SendThreadFunc()
        {
            continueSend = true;
            while (continueSend)
            {
                if (this.socketClient.Isconnect())
                    this.SocketInfo.Stopflag = false;

                if (firstflag)
                {
                    //send heartbeat

                    byte[] data = System.Text.Encoding.Default.GetBytes(sendContent);
                    data = ParseUtil.ToByesByHex(sendContent);
                    socketClient.Send(data);

                    this.SocketInfo.Data = sendContent;
                    this.SocketInfo.IsRefreshSend = true;
                    Thread.Sleep(1000);
                    firstflag = false;
                }

                if (datalist.Count == 0) return;

                for (int j = 0; j < datalist.Count; j++)
                {

                    sendContent = datalist[j];

                    byte[] data = System.Text.Encoding.Default.GetBytes(sendContent);
                    data = ParseUtil.ToByesByHex(sendContent);
                    try
                    {
                        socketClient.Send(data);
                        this.SocketInfo.Data = sendContent;
                        this.SocketInfo.IsRefreshSend = true;
                        fcounts = 0;
                        Thread.Sleep(100); //here is very important

                    }
                    catch (Exception ex)
                    {
                        //deal with the error
                        break;
                    }
                    if (IsAutoSend == false)
                        break;
                } //end for
                datalist.Clear(); //release the datalist memory
                Thread.Sleep(sendInterval);

            }
                
        }

        /// <summary>
        /// Get the Send data according to the Config files.
        /// </summary>
        private void GetDataThreadFunc()
        {

            datalist.Clear();
            // how many meters, how many alarms, how many data.
            // assembly frame and load into datalist string.

            int[] alarmcode = new int[] { this.SocketInfo.timeinterval,20 };
            byte[] alarmtype = new byte[] { 0x28, 0x2F };
            //byte[] temp = new byte[] { 11, 22, 33 };
            string msg = "";
            DateTime dtody = DateTime.Now;
            dtody = dtody.Date;

            if (this.SocketInfo.IsAlarm)
            {
                for (int ii = 2; ii < this.SocketInfo.metercounts; ii++)
                {
                    for (int i = 3; i < alarmcode[0]; i++)
                    {

                        msg = Util.ConverByteToString(Util.AssemblyFrameAlarm(this.SocketInfo.Name, Util.IntToHEX(ii), Util.IntToHEX(i), 0x28));
                        datalist.Add(msg);
                    }
                }
            }
            else if (this.SocketInfo.IsDaily)
            {
                //send previouse 30 days of daily frozen data and set the continue
                //daily frozen data make frame
                //DateTime dtody = new DateTime

                for (int ii = 2; ii < this.SocketInfo.metercounts; ii++)
                {
                    for (int i = 30; i > 0; i--)
                    {
                        //add 30 days daily frozen data every meter.
                        //calculate the meternums and send the current day's all daily data.
                        //maybe need divid several frames
                        msg = Util.ConverByteToString(Util.AssemblyFrameDailyFrozen(this.SocketInfo.Name, Util.IntToHEX(ii), Util.IntToHEX(i), 0x28,dtody.AddDays(-i)));
                        
                    }
                }

            }
            else if (this.SocketInfo.IsLoadProfile)
            {
                //load profile data make frame
            }
            else
            {
                //send heart beat
                byte[] hdata = Util.GetByteDataByType(4, 0x01);
                msg = Util.ConverByteToString(Util.AssemblyFrameBase(this.SocketInfo.Name, 0xC9, 0x7D, 0x02, hdata));
                datalist.Add(msg);
            }

            Thread.Sleep(6000);
            GetDataThreadFunc();
        }

        public void sendReplyData(int type,byte seq)
        {

            DateTime dt = DateTime.Now;
            byte[] data = new byte[40];

            switch(type)
            {
                case 1: // time
                    data = Util.AssemblyFrameSendCurrentTime(this.SocketInfo.Name, dt, seq);
                    break;
                case 2: //current readings
                    break;
                case 3: //current relay status
                    break;
                default:
                    break;
            }
            string msg = ParseUtil.ToHexString(data, data.Length);
            
            socketClient.Send(data);
            this.SocketInfo.Data = Util.AddSpaceToFrame(msg);
            this.SocketInfo.IsRefreshSend = true;

        }
        public void ListenMessage(object o, ReceivedEventArgs e)
        {
                
                try
                {
                    //ReceivedHandler d = new ReceivedHandler(ListenMessage);
                    //this.Invoke(d, new object[] { o, e });
                    byte[] aa = e.Data;
                    byte seq;
                    //check the receive buf whether is the read time frame
                    //if it is , set a flag and send immediately reply
                    int beginind = 0;
                    byte[] recBuf = Util.PickFrame(aa, ref beginind, 20);
                    int pointid;
                    byte seqno;

                    if (recBuf != null)
                    {

                        if (Util.CheckCallTimeFrame(this.SocketInfo.Name, recBuf, out seqno)) ;
                            sendReplyData(1, seq);

                        if (Util.CheckCallCurrentReading(this.SocketInfo.Name, recBuf,out pointid,out seqno))
                            sendReplyData(2,seq);
                 
                    }

                    
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
