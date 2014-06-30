namespace SocketTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.deviceTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bukAddClient = new System.Windows.Forms.ToolStripButton();
            this.tsbAddClient = new System.Windows.Forms.ToolStripButton();
            this.tsbAddServer = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbLoadDefault = new System.Windows.Forms.ToolStripButton();
            this.tsbQuit = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceTree
            // 
            this.deviceTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.deviceTree.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.deviceTree.FullRowSelect = true;
            this.deviceTree.HideSelection = false;
            this.deviceTree.HotTracking = true;
            this.deviceTree.ImageIndex = 0;
            this.deviceTree.ImageList = this.imageList1;
            this.deviceTree.LabelEdit = true;
            this.deviceTree.Location = new System.Drawing.Point(-1, 34);
            this.deviceTree.Margin = new System.Windows.Forms.Padding(4);
            this.deviceTree.Name = "deviceTree";
            this.deviceTree.SelectedImageIndex = 0;
            this.deviceTree.ShowNodeToolTips = true;
            this.deviceTree.Size = new System.Drawing.Size(212, 644);
            this.deviceTree.TabIndex = 0;
            this.deviceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.deviceTree_AfterSelect);
            this.deviceTree.DoubleClick += new System.EventHandler(this.deviceTree_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "delete.png");
            this.imageList1.Images.SetKeyName(1, "add-new-paper-file.png");
            this.imageList1.Images.SetKeyName(2, "add-new-window.png");
            this.imageList1.Images.SetKeyName(3, "add-new-tab.png");
            this.imageList1.Images.SetKeyName(4, "error-red-circle.png");
            this.imageList1.Images.SetKeyName(5, "gear-advanced-options.png");
            this.imageList1.Images.SetKeyName(6, "application_cascade.png");
            this.imageList1.Images.SetKeyName(7, "arrow-back-previous.png");
            this.imageList1.Images.SetKeyName(8, "arrow-forward-next.png");
            this.imageList1.Images.SetKeyName(9, "paper-page-square-text-arrows-reload-refresh.png");
            this.imageList1.Images.SetKeyName(10, "jigsaw-puzzle-piece-extension.png");
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(209, 35);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(888, 645);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(880, 616);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "LoadDefault";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bukAddClient,
            this.tsbAddClient,
            this.tsbAddServer,
            this.tsbDelete,
            this.tsbLoadDefault,
            this.tsbQuit,
            this.tsbAbout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1107, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // bukAddClient
            // 
            this.bukAddClient.Image = ((System.Drawing.Image)(resources.GetObject("bukAddClient.Image")));
            this.bukAddClient.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bukAddClient.Name = "bukAddClient";
            this.bukAddClient.Size = new System.Drawing.Size(134, 24);
            this.bukAddClient.Text = "批量添加客户端";
            // 
            // tsbAddClient
            // 
            this.tsbAddClient.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddClient.Image")));
            this.tsbAddClient.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddClient.Name = "tsbAddClient";
            this.tsbAddClient.Size = new System.Drawing.Size(104, 24);
            this.tsbAddClient.Text = "添加客户端";
            // 
            // tsbAddServer
            // 
            this.tsbAddServer.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddServer.Image")));
            this.tsbAddServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddServer.Name = "tsbAddServer";
            this.tsbAddServer.Size = new System.Drawing.Size(119, 24);
            this.tsbAddServer.Text = "添加服务器端";
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(59, 24);
            this.tsbDelete.Text = "删除";
            // 
            // tsbLoadDefault
            // 
            this.tsbLoadDefault.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadDefault.Image")));
            this.tsbLoadDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadDefault.Name = "tsbLoadDefault";
            this.tsbLoadDefault.Size = new System.Drawing.Size(104, 24);
            this.tsbLoadDefault.Text = "加载初始化";
            // 
            // tsbQuit
            // 
            this.tsbQuit.Image = ((System.Drawing.Image)(resources.GetObject("tsbQuit.Image")));
            this.tsbQuit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQuit.Name = "tsbQuit";
            this.tsbQuit.Size = new System.Drawing.Size(89, 24);
            this.tsbQuit.Text = "存盘退出";
            this.tsbQuit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tsbAbout
            // 
            this.tsbAbout.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsbAbout.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsbAbout.Image = ((System.Drawing.Image)(resources.GetObject("tsbAbout.Image")));
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(59, 24);
            this.tsbAbout.Text = "关于";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 690);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.deviceTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCP测试工具（系统工程部2014.06）";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView deviceTree;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddClient;
        private System.Windows.Forms.ToolStripButton tsbAddServer;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.ToolStripButton bukAddClient;
        private System.Windows.Forms.ToolStripButton tsbQuit;
        private System.Windows.Forms.ToolStripButton tsbLoadDefault;
    }
}

