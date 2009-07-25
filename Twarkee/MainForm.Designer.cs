using System.Drawing;

namespace Twarkee
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.newStatusTextBox = new System.Windows.Forms.TextBox();
            this.backgroundGetLatest = new System.ComponentModel.BackgroundWorker();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.backgroundUpdateStatus = new System.ComponentModel.BackgroundWorker();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblCharsLeft = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTimeOfPost = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.picUser = new System.Windows.Forms.PictureBox();
            this.switchTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusText = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // newStatusTextBox
            // 
            this.newStatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.newStatusTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.newStatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newStatusTextBox.ForeColor = System.Drawing.Color.White;
            this.newStatusTextBox.Location = new System.Drawing.Point(5, 5);
            this.newStatusTextBox.MaxLength = 140;
            this.newStatusTextBox.Name = "newStatusTextBox";
            this.newStatusTextBox.Size = new System.Drawing.Size(350, 29);
            this.newStatusTextBox.TabIndex = 0;
            // 
            // backgroundGetLatest
            // 
            this.backgroundGetLatest.WorkerReportsProgress = true;
            this.backgroundGetLatest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundGetLatest_DoWork);
            this.backgroundGetLatest.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundGetLatest_RunWorkerComplete);
            this.backgroundGetLatest.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundGetLatest_ProgressChanged);
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 1200000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // backgroundUpdateStatus
            // 
            this.backgroundUpdateStatus.WorkerReportsProgress = true;
            this.backgroundUpdateStatus.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundUpdateStatus_DoWork);
            this.backgroundUpdateStatus.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundUpdateStatus_RunWorkerCompleted);
            this.backgroundUpdateStatus.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundUpdateStatus_ProgressChanged);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // lblCharsLeft
            // 
            this.lblCharsLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCharsLeft.AutoSize = true;
            this.lblCharsLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharsLeft.ForeColor = System.Drawing.Color.DimGray;
            this.lblCharsLeft.Location = new System.Drawing.Point(361, 5);
            this.lblCharsLeft.Name = "lblCharsLeft";
            this.lblCharsLeft.Size = new System.Drawing.Size(57, 24);
            this.lblCharsLeft.TabIndex = 6;
            this.lblCharsLeft.Text = "-140-";
            this.lblCharsLeft.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.panel1.Controls.Add(this.statusText);
            this.panel1.Controls.Add(this.lblTimeOfPost);
            this.panel1.Controls.Add(this.lblUserName);
            this.panel1.Controls.Add(this.picUser);
            this.panel1.Location = new System.Drawing.Point(5, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(416, 56);
            this.panel1.TabIndex = 7;
            // 
            // lblTimeOfPost
            // 
            this.lblTimeOfPost.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOfPost.ForeColor = System.Drawing.Color.White;
            this.lblTimeOfPost.Location = new System.Drawing.Point(335, 2);
            this.lblTimeOfPost.Name = "lblTimeOfPost";
            this.lblTimeOfPost.Size = new System.Drawing.Size(83, 19);
            this.lblTimeOfPost.TabIndex = 3;
            this.lblTimeOfPost.Text = "Time of post";
            this.lblTimeOfPost.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(50, 2);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(140, 23);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "username";
            this.lblUserName.Click += new System.EventHandler(this.lblUserName_Click);
            // 
            // picUser
            // 
            this.picUser.Location = new System.Drawing.Point(0, 3);
            this.picUser.Name = "picUser";
            this.picUser.Size = new System.Drawing.Size(48, 48);
            this.picUser.TabIndex = 0;
            this.picUser.TabStop = false;
            // 
            // switchTimer
            // 
            this.switchTimer.Enabled = true;
            this.switchTimer.Interval = 8000;
            this.switchTimer.Tick += new System.EventHandler(this.switchTimer_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 48);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click_1);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusText
            // 
            this.statusText.ForeColor = System.Drawing.Color.White;
            this.statusText.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.statusText.LinkColor = System.Drawing.Color.Lime;
            this.statusText.Location = new System.Drawing.Point(50, 24);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(368, 27);
            this.statusText.TabIndex = 4;
            this.statusText.Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor i" +
                "ncididunt ut labore et dolore magna aliqua. Ut enim ad min";
            this.statusText.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.statusText_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(34)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(430, 103);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.newStatusTextBox);
            this.Controls.Add(this.lblCharsLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "TeleTwitter";
            this.TopMost = true;
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.MouseEnter += new System.EventHandler(this.MainForm_MouseEnter);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.MouseLeave += new System.EventHandler(this.MainForm_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox newStatusTextBox;
		private System.ComponentModel.BackgroundWorker backgroundGetLatest;
        private System.Windows.Forms.Timer refreshTimer;
        private System.ComponentModel.BackgroundWorker backgroundUpdateStatus;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label lblCharsLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picUser;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblTimeOfPost;
        private System.Windows.Forms.Timer switchTimer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.LinkLabel statusText;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
