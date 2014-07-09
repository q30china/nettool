using System;
using System.Collections.Generic;

namespace SocketTool.Core
{
    [Serializable]
    public class InitPara
    {
        public string initServerIp { get; set; }
        public int initServerPort { get; set; }
        public string initClientCounts { get; set; }
        public Boolean IsStart { get; set; }

        public Boolean IsGui { get; set; }

        public Boolean IsAlarm { get; set; }
        public Boolean IsDaily { get; set; }
        public Boolean IsLoadProfile { get; set; }

        public int metercounts { get; set; }
        public int timeinterval { get; set; }

        public InitPara()
        {
            initServerPort = 9131;
            initServerIp = "172.16.201.141";
            initClientCounts = "10";
            IsStart = true;
            IsGui = false;
        }
    }

}
