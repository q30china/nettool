using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SocketTool.Core;

namespace SocketTool
{
    public partial class ConfDialog : Form
    {
        public ConfDialog()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 保存批量配置窗体后需要进行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveConfDialog_Click(object sender, EventArgs e)
        {
            var mainform = (MainForm)Owner;
            SocketInfo si = new SocketInfo();

            mainform.ipara.initServerIp = txtInitSerIp.Text;
            mainform.ipara.initServerPort = Int32.Parse(txtInitPort.Text);
            mainform.ipara.initClientCounts = txtClientCounts.Text;
            mainform.ipara.IsStart = chkConnAll.Checked;
            if (chkGui.Checked)
                mainform.ipara.IsGui = true;
            else
                mainform.ipara.IsGui = false;
            Close();

            if (chkConnAll.Checked)
                mainform.ipara.IsStart = true;
            else
                mainform.ipara.IsStart = false;

            //Initialize the Client Form Parameters
            si.ServerIp = mainform.ipara.initServerIp;
            si.Port = mainform.ipara.initServerPort;
            si.Interval = txtSendDelay.Text;
            si.Format = "Hex";
            si.Protocol = "Tcp";
 
            for (int i = 0; i < Int32.Parse(mainform.ipara.initClientCounts); i++)
            {
                byte[] hdata = Util.GetByteDataByType(4, 0x01);
                si.Name = (Int32.Parse(txtAddress.Text) + i).ToString();
                //根据集中器号进行组帧
                si.Data = Util.ConverByteToString(Util.AssemblyFrameBase(si.Name, 0xC9, 0x7D, 0x02, hdata));

                si.IsAuto = chkConnAll.Checked;
                mainform.BulkAddClientFormNode(si.Name, si);
            }
            mainform.Refresh();
            
        }

        private void btCancelConfDialog_Click(object sender, EventArgs e)
        {
              Close();
        }

        private void ConfDialog_Load(object sender, EventArgs e)
        {
            var mainform = (MainForm)Owner;
            if (mainform.ipara != null)
            {
                txtInitSerIp.Text = mainform.ipara.initServerIp;
                txtInitPort.Text = mainform.ipara.initServerPort.ToString();
                txtClientCounts.Text = mainform.ipara.initClientCounts;
                chkConnAll.Checked = mainform.ipara.IsStart;
            }
        }

    }
}
