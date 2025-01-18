
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
            this.checkboxTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.delayFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.versionTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.labelVersion = new System.Windows.Forms.Label();
            this.buttonCheckForUpdate = new System.Windows.Forms.Button();
            this.comboBoxFFXIV = new System.Windows.Forms.ComboBox();
            this.delayTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.checkboxTableLayout.SuspendLayout();
            this.delayFlowLayout.SuspendLayout();
            this.versionTableLayout.SuspendLayout();
            this.mainTableLayout.SuspendLayout();
            this.delayTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonInject
            // 
            this.buttonInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInject.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInject.Location = new System.Drawing.Point(5, 433);
            this.buttonInject.Margin = new System.Windows.Forms.Padding(5);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(486, 163);
            this.buttonInject.TabIndex = 0;
            this.buttonInject.Text = "注入灵魂";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.ButtonInject_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(318, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(169, 37);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "加入QQ频道";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBoxAutoInject
            // 
            this.checkBoxAutoInject.AutoSize = true;
            this.checkBoxAutoInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxAutoInject.Location = new System.Drawing.Point(246, 3);
            this.checkBoxAutoInject.Name = "checkBoxAutoInject";
            this.checkBoxAutoInject.Size = new System.Drawing.Size(237, 44);
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
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 84);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(144, 40);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(144, 40);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(3, 3);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(237, 44);
            this.checkBoxAutoStart.TabIndex = 5;
            this.checkBoxAutoStart.Text = "开机启动";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            this.checkBoxAutoStart.CheckedChanged += new System.EventHandler(this.checkBoxAutoStart_CheckedChanged);
            // 
            // labelVer
            // 
            this.labelVer.AutoSize = true;
            this.labelVer.Location = new System.Drawing.Point(3, 0);
            this.labelVer.Name = "labelVer";
            this.labelVer.Size = new System.Drawing.Size(100, 37);
            this.labelVer.TabIndex = 8;
            this.labelVer.Text = "default";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(3, 0);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(129, 37);
            this.delayLabel.TabIndex = 10;
            this.delayLabel.Text = "延迟注入";
            // 
            // second
            // 
            this.second.AutoSize = true;
            this.second.Location = new System.Drawing.Point(248, 0);
            this.second.Name = "second";
            this.second.Size = new System.Drawing.Size(45, 37);
            this.second.TabIndex = 11;
            this.second.Text = "秒";
            // 
            // delayBox
            // 
            this.delayBox.Location = new System.Drawing.Point(138, 3);
            this.delayBox.Name = "delayBox";
            this.delayBox.Size = new System.Drawing.Size(104, 43);
            this.delayBox.TabIndex = 12;
            this.delayBox.ValueChanged += new System.EventHandler(this.delayBox_ValueChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(8, 667);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(496, 46);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.66667F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(160, 32);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 35);
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Location = new System.Drawing.Point(3, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(480, 44);
            this.label1.TabIndex = 14;
            this.label1.Text = "自动注入黑屏请先尝试增加延迟！！";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.label1, "别问了别问了");
            // 
            // checkBoxSafeMode
            // 
            this.checkBoxSafeMode.AutoSize = true;
            this.checkBoxSafeMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxSafeMode.Location = new System.Drawing.Point(3, 53);
            this.checkBoxSafeMode.Name = "checkBoxSafeMode";
            this.checkBoxSafeMode.Size = new System.Drawing.Size(237, 44);
            this.checkBoxSafeMode.TabIndex = 15;
            this.checkBoxSafeMode.Text = "禁用所有插件";
            this.toolTip1.SetToolTip(this.checkBoxSafeMode, "禁用所有插件，以避免爆炸");
            this.checkBoxSafeMode.UseVisualStyleBackColor = true;
            this.checkBoxSafeMode.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkboxTableLayout
            // 
            this.checkboxTableLayout.ColumnCount = 2;
            this.checkboxTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.checkboxTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.checkboxTableLayout.Controls.Add(this.checkBoxAutoStart, 0, 0);
            this.checkboxTableLayout.Controls.Add(this.checkBoxAutoInject, 1, 0);
            this.checkboxTableLayout.Controls.Add(this.checkBoxSafeMode, 0, 1);
            this.checkboxTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkboxTableLayout.Location = new System.Drawing.Point(5, 213);
            this.checkboxTableLayout.Margin = new System.Windows.Forms.Padding(5);
            this.checkboxTableLayout.Name = "checkboxTableLayout";
            this.checkboxTableLayout.RowCount = 2;
            this.checkboxTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.checkboxTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.checkboxTableLayout.Size = new System.Drawing.Size(486, 100);
            this.checkboxTableLayout.TabIndex = 16;
            // 
            // delayFlowLayout
            // 
            this.delayFlowLayout.Controls.Add(this.delayLabel);
            this.delayFlowLayout.Controls.Add(this.delayBox);
            this.delayFlowLayout.Controls.Add(this.second);
            this.delayFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.delayFlowLayout.Location = new System.Drawing.Point(3, 3);
            this.delayFlowLayout.Name = "delayFlowLayout";
            this.delayFlowLayout.Size = new System.Drawing.Size(480, 50);
            this.delayFlowLayout.TabIndex = 17;
            // 
            // versionTableLayout
            // 
            this.versionTableLayout.ColumnCount = 2;
            this.versionTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.versionTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.versionTableLayout.Controls.Add(this.labelVer, 0, 0);
            this.versionTableLayout.Controls.Add(this.linkLabel1, 1, 0);
            this.versionTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versionTableLayout.Location = new System.Drawing.Point(3, 604);
            this.versionTableLayout.Name = "versionTableLayout";
            this.versionTableLayout.RowCount = 1;
            this.versionTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.versionTableLayout.Size = new System.Drawing.Size(490, 52);
            this.versionTableLayout.TabIndex = 18;
            // 
            // mainTableLayout
            // 
            this.mainTableLayout.ColumnCount = 1;
            this.mainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayout.Controls.Add(this.labelVersion, 0, 0);
            this.mainTableLayout.Controls.Add(this.buttonCheckForUpdate, 0, 1);
            this.mainTableLayout.Controls.Add(this.versionTableLayout, 0, 6);
            this.mainTableLayout.Controls.Add(this.comboBoxFFXIV, 0, 2);
            this.mainTableLayout.Controls.Add(this.checkboxTableLayout, 0, 3);
            this.mainTableLayout.Controls.Add(this.buttonInject, 0, 5);
            this.mainTableLayout.Controls.Add(this.delayTableLayout, 0, 4);
            this.mainTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayout.Location = new System.Drawing.Point(8, 8);
            this.mainTableLayout.Name = "mainTableLayout";
            this.mainTableLayout.RowCount = 7;
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTableLayout.Size = new System.Drawing.Size(496, 659);
            this.mainTableLayout.TabIndex = 19;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(5, 5);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(5);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(262, 37);
            this.labelVersion.TabIndex = 1;
            this.labelVersion.Text = "当前版本 : Unknown";
            // 
            // buttonCheckForUpdate
            // 
            this.buttonCheckForUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCheckForUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCheckForUpdate.Location = new System.Drawing.Point(5, 52);
            this.buttonCheckForUpdate.Margin = new System.Windows.Forms.Padding(5);
            this.buttonCheckForUpdate.Name = "buttonCheckForUpdate";
            this.buttonCheckForUpdate.Size = new System.Drawing.Size(486, 96);
            this.buttonCheckForUpdate.TabIndex = 0;
            this.buttonCheckForUpdate.Text = "检查更新";
            this.buttonCheckForUpdate.UseVisualStyleBackColor = true;
            this.buttonCheckForUpdate.Click += new System.EventHandler(this.ButtonCheckForUpdate_Click);
            // 
            // comboBoxFFXIV
            // 
            this.comboBoxFFXIV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxFFXIV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFFXIV.FormattingEnabled = true;
            this.comboBoxFFXIV.ImeMode = System.Windows.Forms.ImeMode.On;
            this.comboBoxFFXIV.ItemHeight = 37;
            this.comboBoxFFXIV.Location = new System.Drawing.Point(5, 158);
            this.comboBoxFFXIV.Margin = new System.Windows.Forms.Padding(5);
            this.comboBoxFFXIV.Name = "comboBoxFFXIV";
            this.comboBoxFFXIV.Size = new System.Drawing.Size(486, 45);
            this.comboBoxFFXIV.TabIndex = 2;
            this.comboBoxFFXIV.Click += new System.EventHandler(this.comboBoxFFXIV_Clicked);
            // 
            // delayTableLayout
            // 
            this.delayTableLayout.ColumnCount = 1;
            this.delayTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.delayTableLayout.Controls.Add(this.label1, 0, 1);
            this.delayTableLayout.Controls.Add(this.delayFlowLayout, 0, 0);
            this.delayTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delayTableLayout.Location = new System.Drawing.Point(5, 323);
            this.delayTableLayout.Margin = new System.Windows.Forms.Padding(5);
            this.delayTableLayout.Name = "delayTableLayout";
            this.delayTableLayout.RowCount = 2;
            this.delayTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.delayTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.delayTableLayout.Size = new System.Drawing.Size(486, 100);
            this.delayTableLayout.TabIndex = 19;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 721);
            this.Controls.Add(this.mainTableLayout);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(540, 800);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Text = "卫月更新器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Disposed += new System.EventHandler(this.FormMain_Disposed);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.delayBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.checkboxTableLayout.ResumeLayout(false);
            this.checkboxTableLayout.PerformLayout();
            this.delayFlowLayout.ResumeLayout(false);
            this.delayFlowLayout.PerformLayout();
            this.versionTableLayout.ResumeLayout(false);
            this.versionTableLayout.PerformLayout();
            this.mainTableLayout.ResumeLayout(false);
            this.mainTableLayout.PerformLayout();
            this.delayTableLayout.ResumeLayout(false);
            this.delayTableLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.TableLayoutPanel checkboxTableLayout;
        private System.Windows.Forms.FlowLayoutPanel delayFlowLayout;
        private System.Windows.Forms.TableLayoutPanel versionTableLayout;
        private System.Windows.Forms.TableLayoutPanel mainTableLayout;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Button buttonCheckForUpdate;
        private System.Windows.Forms.ComboBox comboBoxFFXIV;
        private System.Windows.Forms.TableLayoutPanel delayTableLayout;
    }
}

