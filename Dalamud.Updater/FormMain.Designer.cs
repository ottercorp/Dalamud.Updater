
using System;

namespace Dalamud.Updater
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonCheckForUpdate = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.comboBoxFFXIV = new System.Windows.Forms.ComboBox();
            this.buttonInject = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBoxAutoInject = new System.Windows.Forms.CheckBox();
            this.DalamudUpdaterIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.labelVer = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.second = new System.Windows.Forms.Label();
            this.delayBox = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxSafeMode = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCheckForUpdate
            // 
            this.buttonCheckForUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheckForUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCheckForUpdate.Location = new System.Drawing.Point(12, 34);
            this.buttonCheckForUpdate.Name = "buttonCheckForUpdate";
            this.buttonCheckForUpdate.Size = new System.Drawing.Size(195, 40);
            this.buttonCheckForUpdate.TabIndex = 0;
            this.buttonCheckForUpdate.Text = "检查更新";
            this.buttonCheckForUpdate.UseVisualStyleBackColor = true;
            this.buttonCheckForUpdate.Click += new System.EventHandler(this.ButtonCheckForUpdate_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(12, 9);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(119, 15);
            this.labelVersion.TabIndex = 1;
            this.labelVersion.Text = "当前版本 : Unknown";
            // 
            // comboBoxFFXIV
            // 
            this.comboBoxFFXIV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFFXIV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFFXIV.FormattingEnabled = true;
            this.comboBoxFFXIV.ImeMode = System.Windows.Forms.ImeMode.On;
            this.comboBoxFFXIV.Location = new System.Drawing.Point(12, 80);
            this.comboBoxFFXIV.Name = "comboBoxFFXIV";
            this.comboBoxFFXIV.Size = new System.Drawing.Size(195, 23);
            this.comboBoxFFXIV.TabIndex = 2;
            this.comboBoxFFXIV.Click += new System.EventHandler(this.comboBoxFFXIV_Clicked);
            // 
            // buttonInject
            // 
            this.buttonInject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInject.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInject.Location = new System.Drawing.Point(11, 213);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(196, 79);
            this.buttonInject.TabIndex = 0;
            this.buttonInject.Text = "注入灵魂";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.ButtonInject_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(134, 295);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(77, 15);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "加入QQ频道";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBoxAutoInject
            // 
            this.checkBoxAutoInject.AutoSize = true;
            this.checkBoxAutoInject.Location = new System.Drawing.Point(130, 112);
            this.checkBoxAutoInject.Name = "checkBoxAutoInject";
            this.checkBoxAutoInject.Size = new System.Drawing.Size(78, 19);
            this.checkBoxAutoInject.TabIndex = 4;
            this.checkBoxAutoInject.Text = "自动注入";
            this.checkBoxAutoInject.UseVisualStyleBackColor = true;
            this.checkBoxAutoInject.CheckedChanged += new System.EventHandler(this.checkBoxAutoInject_CheckedChanged);
            // 
            // DalamudUpdaterIcon
            // 
            this.DalamudUpdaterIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.DalamudUpdaterIcon.BalloonTipText = "123";
            this.DalamudUpdaterIcon.BalloonTipTitle = "卫月更新器";
            this.DalamudUpdaterIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.DalamudUpdaterIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("DalamudUpdaterIcon.Icon")));
            this.DalamudUpdaterIcon.Text = "DalamudUpdater";
            this.DalamudUpdaterIcon.Visible = true;
            this.DalamudUpdaterIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DalamudUpdaterIcon_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(12, 112);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(78, 19);
            this.checkBoxAutoStart.TabIndex = 5;
            this.checkBoxAutoStart.Text = "开机启动";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            this.checkBoxAutoStart.CheckedChanged += new System.EventHandler(this.checkBoxAutoStart_CheckedChanged);
            // 
            // labelVer
            // 
            this.labelVer.AutoSize = true;
            this.labelVer.Location = new System.Drawing.Point(9, 295);
            this.labelVer.Name = "labelVer";
            this.labelVer.Size = new System.Drawing.Size(44, 15);
            this.labelVer.TabIndex = 8;
            this.labelVer.Text = "default";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(12, 165);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(59, 15);
            this.delayLabel.TabIndex = 10;
            this.delayLabel.Text = "延迟注入";
            // 
            // second
            // 
            this.second.AutoSize = true;
            this.second.Location = new System.Drawing.Point(188, 165);
            this.second.Name = "second";
            this.second.Size = new System.Drawing.Size(20, 15);
            this.second.TabIndex = 11;
            this.second.Text = "秒";
            // 
            // delayBox
            // 
            this.delayBox.Location = new System.Drawing.Point(77, 162);
            this.delayBox.Name = "delayBox";
            this.delayBox.Size = new System.Drawing.Size(104, 23);
            this.delayBox.TabIndex = 12;
            this.delayBox.ValueChanged += new System.EventHandler(this.delayBox_ValueChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 316);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(221, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Location = new System.Drawing.Point(12, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "自动注入黑屏请先尝试增加延迟！！";
            this.toolTip1.SetToolTip(this.label1, "别问了别问了");
            // 
            // checkBoxSafeMode
            // 
            this.checkBoxSafeMode.AutoSize = true;
            this.checkBoxSafeMode.Location = new System.Drawing.Point(11, 137);
            this.checkBoxSafeMode.Name = "checkBoxSafeMode";
            this.checkBoxSafeMode.Size = new System.Drawing.Size(104, 19);
            this.checkBoxSafeMode.TabIndex = 15;
            this.checkBoxSafeMode.Text = "禁用所有插件";
            this.toolTip1.SetToolTip(this.checkBoxSafeMode, "禁用所有插件，以避免爆炸");
            this.checkBoxSafeMode.UseVisualStyleBackColor = true;
            this.checkBoxSafeMode.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 338);
            this.Controls.Add(this.checkBoxSafeMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.delayBox);
            this.Controls.Add(this.second);
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.labelVer);
            this.Controls.Add(this.checkBoxAutoStart);
            this.Controls.Add(this.checkBoxAutoInject);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.comboBoxFFXIV);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.buttonInject);
            this.Controls.Add(this.buttonCheckForUpdate);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "卫月更新器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Disposed += new System.EventHandler(this.FormMain_Disposed);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.delayBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCheckForUpdate;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.ComboBox comboBoxFFXIV;
        private System.Windows.Forms.Button buttonInject;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkBoxAutoInject;
        private System.Windows.Forms.NotifyIcon DalamudUpdaterIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxAutoStart;
        private System.Windows.Forms.Label labelVer;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.Label second;
        private System.Windows.Forms.NumericUpDown delayBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxSafeMode;
    }
}

