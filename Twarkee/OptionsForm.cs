using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Twarkee
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(OptionsForm_Load);
        }

        void OptionsForm_Load(object sender, EventArgs e)
        {
            LoadOptions();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim() == string.Empty || txtPassword.Text.Trim() == string.Empty)
            {
                MessageBox.Show("User name and password are required.", "Account Details required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            int delay = 0;
            if (txtRefreshDelay.Text.Trim() == string.Empty || !int.TryParse(txtRefreshDelay.Text.Trim(), out delay))
            {
                MessageBox.Show("Refresh Delay is required and must be a valid number.", "Refresh Delay required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SaveOptions();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveOptions();
        }

        private void LoadOptions()
        {
            txtUserName.Text = Properties.Settings.Default.UserName;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtRefreshDelay.Text = Properties.Settings.Default.RefreshDelay.ToString();

            //chkMinimizeToTray.Checked = Properties.Settings.Default.MinimizeToTray;
            chkStartAtLogin.Checked = (((string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "TeleTwitter", "")).Length > 0);
            radJsonButton.Checked = (Properties.Settings.Default.ApiType == (int)TeleTwitter.Lib.ApiType.JSON);
            //radXmlButton.Checked = (Properties.Settings.Default.ApiType == (int)TeleTwitter.Lib.ApiType.XML);

            //chkHandlePublicTimeline.Checked = Properties.Settings.Default.UsePublicTimeline;

        }

        private void SaveOptions()
        {
            Properties.Settings.Default.UserName = txtUserName.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.RefreshDelay = int.Parse(txtRefreshDelay.Text.Trim());
            //Properties.Settings.Default.MinimizeToTray = chkMinimizeToTray.Checked;

            //if (Properties.Settings.Default.UsePublicTimeline != chkHandlePublicTimeline.Checked)
            //{
            //    MessageBox.Show("Please close and reopen TeleTwitter for your changes to take place.", "Restart TeleTwitter", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            //Properties.Settings.Default.UsePublicTimeline = chkHandlePublicTimeline.Checked;

            if (chkStartAtLogin.Checked != (((string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "TeleTwitter", "")).Length > 0))
            {
                if (chkStartAtLogin.Checked)
                {
                    Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "TeleTwitter", System.Reflection.Assembly.GetEntryAssembly().Location);
                }
                else
                {
                    Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "TeleTwitter", "");
                }
            }

            Properties.Settings.Default.ApiType = radJsonButton.Checked ? (int)TeleTwitter.Lib.ApiType.JSON : (int)TeleTwitter.Lib.ApiType.XML;
			try
			{
				TeleTwitter.Lib.TwitterManager.Instance().TwitterApi = (TeleTwitter.Lib.ApiType)Properties.Settings.Default.ApiType;
			}
			catch
			{
				// just ignore login errors until after we've saved settings
			}

            Properties.Settings.Default.Save();
        }



    }
}