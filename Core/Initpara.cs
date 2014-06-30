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
        public InitPara()
        {
            initServerPort = 9131;
            initServerIp = "192.168.5.131";
            initClientCounts = "10";
            IsStart = true;
            IsGui = false;
        }
    }

}
