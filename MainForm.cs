using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SocketTool.Core;
using System.Diagnostics;

namespace SocketTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private TreeNode rootNode1;
        private TreeNode rootNode2;
        private int index1 = 1;
        private int index2 = 1;
        private int pageIndex = 0;

        private List<Form> pageList = new List<Form>();
        private List<SocketInfo> socketInfoList = new List<SocketInfo>();

        private List<ClientCmd> cliList = new List<ClientCmd>();

        public InitPara ipara = new InitPara();

        private string XMLFileName = "socketinfo.xml";
        private string XMLInitName = "configure.xml";

        private void Form1_Load(object sender, EventArgs e)
        {

            Boolean loadf = false;
            rootNode1 = new TreeNode("客户终端", 5, 5);
            this.deviceTree.Nodes.Add(rootNode1);
            rootNode2 = new TreeNode("服务器终端", 6, 6);
            this.deviceTree.Nodes.Add(rootNode2);
            
            try
            {
                //启动程序时，提示是否自动load已经保存的配置文件
                //因为大数据量测试时，初始化这个配置文件需要时间，需确认下。也可通过后来的LoadDefault实现
                DialogResult result = MessageBox.Show("Are you want to load and auto start the auto testing ?  ", "Load", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    loadf = false;
                else
                    loadf = true;

                if (loadf)
                {
                    //SocketInfo[] sis = MySerializer.DeSerialize(XMLFileName);
                    socketInfoList = MySerializer.Deserialize<List<SocketInfo>>(XMLFileName);

                    if (System.IO.File.Exists(XMLInitName))
                        ipara = MySerializer.Deserialize<InitPara>(XMLInitName);
                    //SocketInfo[] sis = MySerializer.Deserialize<SocketInfo[]>(XMLFileName);

                    foreach (SocketInfo si in socketInfoList)
                    {
                        if (si.Type == "Server")
                        {
                            AddServerFormNode(si.Name, si);
                        }
                        else
                        {
                            AddClientFormNode(si.Name, si);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            	Debug.WriteLine(ex.Message);
            	Debug.WriteLine(ex.StackTrace);
            }
        }

        private void AddClientFormNode(string name, SocketInfo si)
        {
            TreeNode ch = rootNode1.Nodes.Add(name, name, 7, 7);
            index1++;
            ClientForm form2 = new ClientForm();
            if(si != null)
               form2.SocketInfo = si;
            if (ipara.IsGui) //带界面
            {
                TabPage tp = addPage(name, form2);
                tp.ImageIndex = 2;
                ch.Tag = tp;
            }  //不带界面
            else
            {
                ClientCmd cli = new ClientCmd();
                cli.SocketInfo = si.ShallowCopy();
                cliList.Add(cli);
                cli.SendData(null,null);
            }
            rootNode1.ExpandAll();
            deviceTree.SelectedNode = ch;

            if (ipara.IsGui)
            {
                if (si == null)
                    return;

                if (si.IsAuto == true)
                {
                    form2.btnAutoSend();
                }
            }
        }

        private void AddServerFormNode(string name, SocketInfo si)
        {
            TreeNode ch = rootNode2.Nodes.Add(name, name, 8, 8);
            index2++;

            ServerForm form2 = new ServerForm();
            if (si != null)
                form2.SocketInfo = si;
            TabPage tp = addPage(name, form2);
            ch.Tag = tp;
            tp.ImageIndex = 3;
            rootNode2.ExpandAll();
            deviceTree.SelectedNode = ch;
        }

        /// <summary>
        /// 主界面工具栏选中调用函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Name == "tsbAddClient")
            {
                string name = "客户端" + index1;
                if (ipara.IsGui)
                    AddClientFormNode(name, null);
                else
                {
                    MessageBox.Show("Now,you are in CMD(No GUI) mode, Can't Add here!");
                    return;
                }
            }
            else if (e.ClickedItem.Name == "tsbAddServer")
            {
                string name = "服务器端" + index2;
                AddServerFormNode(name, null);
                //if (ipara.IsGui)
                //    AddServerFormNode(name, null);
                //else
                //{
                //    MessageBox.Show("Now,you are in CMD(No GUI) mode,Can't Add here!");
                //    return;
                //}
            }
            else if (e.ClickedItem.Name == "tsbAbout")
            {
                MessageBox.Show("HeXing TCP/IP Test Tools \nVersion: 1.0.0.0 \nSystem Engineering Department \n\n\n                          joey  2014.6.30", "About", MessageBoxButtons.OK);
                //Process.Start("iexplore.exe", "www.hxgroup.co");
            }
            else if (e.ClickedItem.Name == "tsbDelete")
            {
                TreeNode tn = this.deviceTree.SelectedNode;

                if (tn.Level < 1)
                {
                    return;
                }

                if (ipara.IsGui)
                {
                    TabPage tp = (TabPage)tn.Tag;
                    this.tabControl1.TabPages.Remove(tp);
                }
                else
                {
                    cliList.RemoveAt(deviceTree.SelectedNode.Index);
                }

                tn.Parent.Nodes.Remove(tn);
            }
            else
            {
                if (e.ClickedItem.Name == "bukAddClient")
                {
                    var confd = new ConfDialog();
                    confd.Owner = this;
                    confd.ShowDialog();
                    confd.Dispose();
                }
                else if (e.ClickedItem.Name == "tsbQuit")
                {

                      quit();
                }
                else if (e.ClickedItem.Name == "tsbLoadDefault")
                {
                    if( tabControl1.TabCount == 1 ) 
                      load();
                }
                else if (e.ClickedItem.Name == "tsbRefresh")
                {
                    //for (TreeNode tn in deviceTree)
                    //deviceTree.SelectedNode.ImageIndex = 4;
                    foreach (TreeNode tn in this.deviceTree.Nodes)
                    {
                        foreach (TreeNode ctn in tn.Nodes)
                        {
                            for (int i = 0; i < cliList.Count; i++)
                            {
                                if (cliList[i].SocketInfo.Name == ctn.Name)
                                {
                                    if (cliList[i].SocketInfo.Stopflag == true)
                                        ctn.ImageIndex = 4;
                                    else
                                        ctn.ImageIndex = 7;

                                }

                            }
                        }
                    }
                }
            }
        }

        private TabPage addPage(string pageText, Form form)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            TabPage Page = this.tabPage1;
            if (pageIndex == 0)
            {
                Page.Controls.Add(form);
            }
            else
            {                
                Page = new TabPage();
                Page.ImageIndex = 3;
                Page.Name = "Page" + pageIndex.ToString();
                Page.TabIndex = pageIndex;
                this.tabControl1.Controls.Add(Page);
                Page.Controls.Add(form);
                this.tabControl1.SelectedTab = Page;

            }
            Page.Text = pageText;
            pageIndex++;

            form.Show();
            pageList.Add(form);
            return Page;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void deviceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                TabPage tp = (TabPage)e.Node.Tag;

                this.tabControl1.SelectedTab = tp;
            }
        }

        /// <summary>
        /// 批量添加TCP测试客户端，可以带界面也可不带界面
        /// 如果带界面，由于Add TabPage有资源限制，所以大数据测试需要CMD方式，不带界面连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cli"></param>
        public void BulkAddClientFormNode(string name, SocketInfo cli)
        {
            AddClientFormNode(name, cli);        
        }

        /// <summary>
        /// LoadDefault按钮调用，将保存的客户端和服务器信息Load进界面
        /// </summary>
        private void load()
        {
            socketInfoList = MySerializer.Deserialize<List<SocketInfo>>(XMLFileName);

            if (System.IO.File.Exists(XMLInitName))
                ipara = MySerializer.Deserialize<InitPara>(XMLInitName);

            foreach (SocketInfo si in socketInfoList)
            {
                if (si.Type == "Server")
                {
                    AddServerFormNode(si.Name, si);
                }
                else
                {
                    AddClientFormNode(si.Name, si);
                }
            }
        }

        /// <summary>
        /// 此函数作为对连接客户端配置进行保存时，调用。
        /// 注意，带界面（右侧的TabPage页面）和不带界面时，始终只能保存一种类型，会替换保存内容
        /// </summary>
        private void quit()
        {
            socketInfoList = new List<SocketInfo>();
            //不带界面操作时，保存所有的ClientCmd链表保存的内容
            if(!ipara.IsGui)
            {
                for (int i = 0; i < cliList.Count; i++)
                {
                    SocketInfo iss = new SocketInfo();
                    iss = cliList[i].SocketInfo.ShallowCopy();
                    socketInfoList.Add(iss);

                }
            }
            else //带界面操作时，保存右侧Tabpage中的内容
            {
                foreach (TreeNode tn in this.deviceTree.Nodes)
                {
                    foreach (TreeNode ctn in tn.Nodes)
                    {

                        TabPage tp = (TabPage)ctn.Tag;
                        if (tp != null)
                            tp.Text = ctn.Text;

                        if (ipara.IsGui)
                        {
                            if (tp == null)
                                continue;
                            foreach (Form f in tp.Controls)
                            {
                                SocketInfo iss = new SocketInfo();
                                ISocketInfo isi = (ISocketInfo)f;
                                isi.SocketInfo.Name = tp.Text;
                                //here, please don't using = to assign object value , coz only pass the reference. joey 2014.6.27
                                //iss = isi.SocketInfo;
                                f.Close();
                                iss = isi.SocketInfo.ShallowCopy();
                                socketInfoList.Add(iss);
                            }
                        }
                    }
                }
            }

            MySerializer.Serialize(socketInfoList, XMLFileName);
            MySerializer.Serialize(ipara, XMLInitName);
            //彻底退出应用程序
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 双击某个客户端连接，右侧TabPage显示对应连接信息和报文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deviceTree_DoubleClick(object sender, EventArgs e)
        {
            //遍历后边的tab页，如果有do nothing
            foreach (TabPage f in tabControl1.Controls)
            {
                if (deviceTree.SelectedNode.Name == f.Text)
                {
                    tabControl1.SelectedTab = f;
                    
                    return;
                }
            }
            //for(int i=0; i<cliList.Count; i++)
            //{
            //    if (cliList[i].SocketInfo.Name == deviceTree.SelectedNode.Name)
            //    {
            //        AddClientTab(cliList[i].SocketInfo.Name, cliList[i].SocketInfo);

            //    }

            //}
            //替代循环查找代码，效率更高
            int ind = deviceTree.SelectedNode.Index;
            AddClientTab(cliList[ind].SocketInfo.Name, cliList[ind].SocketInfo);
        }

        /// <summary>
        /// 增加一个客户端的Tab页
        /// </summary>
        /// <param name="name"></param>
        /// <param name="si"></param>
        private void AddClientTab(string name, SocketInfo si)
        {
        
            ClientForm form = new ClientForm();
            if (si != null)
                form.SocketInfo = si;

            TabPage tp = addPage(name, form);
                tp.ImageIndex = 2;
            tabControl1.SelectedTab = tp;
            form.RefreshData();
   
        }
    }
}
