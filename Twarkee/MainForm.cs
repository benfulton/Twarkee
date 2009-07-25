using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using Genghis;
using SpeechLib;
using System.Drawing;
using TeleTwitter.Lib;
using System.Drawing.Drawing2D;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Net;
using Twarkee.Properties;

namespace Twarkee
{
    public partial class MainForm : Form
    {
        private bool userNameSetFlag = false;
        private BrowserController _controller = new BrowserController();
        private WindowSerializer _windowSerializer = null;
        private TimeSpan _timeRemaining;
    	private SpeechVoiceSpeakFlags _speechFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
    	private SpVoice _voice;
        private List<Timeline> _timelines = new List<Timeline>();
        private bool _hasUpdates = false;

        public MainForm()
        {
            InitializeComponent();
            _windowSerializer = new WindowSerializer(this);
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Check the configuration settings
            CheckConfiguration();

            // Initialize the controls
            InitControls();

            // Init Speech stuff
            _voice = new SpVoice();
            _voice.Volume = Settings.Default.VoiceVolume;
            _voice.Rate = Settings.Default.VoiceRate;

            try
            {
                if (!(string.IsNullOrEmpty(Settings.Default.SelectedVoice)) && Settings.Default.SelectedVoice != "Default")
                {
                    _voice.Voice = _voice.GetVoices("Name=" + Settings.Default.SelectedVoice, "").Item(0);
                }
            }
            catch
            {
                //do nothing, just use the default voice
            }

            notifyIcon.Text = this.Text;
            notifyIcon.Icon = this.Icon;

            RememberTheMilkRequest.sharedSecret = "ee19a2728ff74946";
            RememberTheMilkRequest.APIKey = "34ce6586ecb33100b417182d7de1102f";

            /*
            if (Settings.Default.RememberTheMilkReminders)
            {
                if (String.IsNullOrEmpty(Settings.Default.RememberTheMilkToken))
                {
                    string authPage = new RememberTheMilkManager().GetAuthenticationPage();
                    System.Diagnostics.Process.Start(authPage);
                    MessageBox.Show("Authenticate to Remember the Milk?");
                }
            }
             * */
        }

         #region Config/Init Support

        private void CheckConfiguration()
        {
            try
            {
                // Make sure the user has entered a username and password.
                if (Settings.Default.UserName == string.Empty || Settings.Default.Password == string.Empty)
                {
                    switchTimer.Enabled = false;
                    using (OptionsForm optionsForm = new OptionsForm())
                    {
                        optionsForm.ShowDialog(this);
                    }
                    switchTimer.Enabled = true;
                }
            }
            catch( Exception ex )
            {
            }
        }

        List<Status> items;
        int current;

        private void InitControls()
        {
            try
            {
                // Setting default to XML. I think the JSON stuff is having issues periodically.
                TwitterManager.Instance().TwitterApi = (ApiType)Properties.Settings.Default.ApiType;
            }
            catch
            {
            }
			TwitterManager.Instance().Username = Properties.Settings.Default.UserName;
			TwitterManager.Instance().Password = Properties.Settings.Default.Password;

            // Setup the text box
            _controller.UserNameChanged +=new BrowserController.UserNameChangedHandler(_controller_UserNameChanged);

            // Setup the web browser
            //webBrowser1.ObjectForScripting = _controller;
            //webBrowser1.DocumentText = _controller.GetTimelines();

            // Initialize the timelines we'll be tracking.
            Timeline t = new Timeline();
            t.TimelineName = "Friends";
            t.ThisTimelineType = TimelineType.AllFriends;
            t.HtmlContainerId = "tlcontainer";
            _timelines.Add(t);

            items = _controller.GetTimeline(t);
            current = items.Count-1;
            UpdateStatus(current);

            // Get the current tweets and set the refresher
            backgroundGetLatest.RunWorkerAsync();
			_timeRemaining = new TimeSpan(0, Settings.Default.RefreshDelay, 0);
			refreshTimer.Interval = 1000;
        	refreshTimer.Enabled = true;

            

            // Setup the menu
            newStatusTextBox.TextChanged += new EventHandler(newStatusTextBox_TextChanged);
            newStatusTextBox.KeyDown += new KeyEventHandler(newStatusTextBox_Key);
            newStatusTextBox.KeyUp += new KeyEventHandler(newStatusTextBox_KeyUp);
            newStatusTextBox.KeyPress += new KeyPressEventHandler(newStatusTextBox_KeyPress);
            newStatusTextBox.MaxLength = 140;
        	RefreshRemainingCharacters();
        }

        HybridDictionary images = new HybridDictionary();

        private void FindHyperlinks( LinkLabel label )
        {
            Regex urlregex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)",
                             RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = urlregex.Matches(label.Text);

            label.Links.Clear();
            foreach( Match m in mc )
            {
                label.Links.Add( new LinkLabel.Link( m.Index, m.Length ));
            }

            Regex atregex = new Regex(@"(@(\/?)\S*)",
                             RegexOptions.IgnoreCase | RegexOptions.Compiled);
            mc = atregex.Matches(label.Text);

            foreach (Match m in mc)
            {
                label.Links.Add(new LinkLabel.Link(m.Index, m.Length));
            }
        }
        private void UpdateStatus(int i)
        {
            statusText.Text = items[i].Text;
            FindHyperlinks(statusText);
            User usr = items[i].User;
            lblUserName.Text = usr.ScreenName;
            lblTimeOfPost.Text = items[i].CreatedAt.ToShortTimeString();
            toolTip1.SetToolTip(picUser, usr.Name + "\r\n" + usr.Description);

            if (statusText.Text.Contains(Settings.Default.UserName))
                BackColor = Color.Red;
            else if (DateTime.Now - items[i].CreatedAt < new TimeSpan(0, 5, 0))
                BackColor = Color.FromArgb(0xC5, 0xC5, 0xC5);
            else
                BackColor = Color.FromArgb(27, 34, 40);

            if (images.Contains(usr.ScreenName))
            {
                picUser.Image = (Image) images[usr.ScreenName];

            }
            else
                picUser.LoadAsync(usr.ProfileImageUrl);
        }

        #endregion

        #region Status Box Support

        //lets us know if the user has entered any text into the textbox manually
        void newStatusTextBox_TextChanged(object sender, EventArgs e)
        {
            userNameSetFlag = newStatusTextBox.Text.Length > 0;
        }

        void newStatusTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(1))
            {
                newStatusTextBox.SelectAll();
                e.Handled = true;
            }
        }

        //Wires up the interaction from the controller to winform. We want to 
        //allow the user to click on differnt avtars, but we do not want them to overwrite
        //any text they manually entered.
        void _controller_UserNameChanged(object sender, BrowserController.UserNameChangeEventArguments e)
        {
            string username = sender as string;
            if (username != null)
            {
                if(!userNameSetFlag)
                {
                    string mask = "{0}{1} : ";
                    newStatusTextBox.Text = string.Format(mask, e.CommandText, username);

                    userNameSetFlag = false;
                    newStatusTextBox.Focus();
                    newStatusTextBox.Select(newStatusTextBox.Text.Length, 0);
                }
            }
        }

        #endregion

        #region Menu Support

        void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!backgroundGetLatest.IsBusy)
            {
                backgroundGetLatest.RunWorkerAsync();
            }
        }

        static void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OptionsForm form = new OptionsForm())
            {
                form.ShowDialog();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (HelpForm form = new HelpForm())
            {
                form.ShowDialog();
            }
        }

        #endregion

        #region Update Status

        static void BackUpMessage(string text)
        {
            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "message.log"), true))
            {
                streamWriter.WriteLine("Message sent: " + DateTime.Now);
                streamWriter.WriteLine(text);
                streamWriter.WriteLine();
            }
        }

        void newStatusTextBox_Key(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;

                if (newStatusTextBox.Text.Length > 2)
                {
                    string text = newStatusTextBox.Text;
                    BackUpMessage(text);
                    newStatusTextBox.Clear();
                    newStatusTextBox.Refresh();
                    RefreshRemainingCharacters();

                    if (!backgroundUpdateStatus.IsBusy)
                    {
                        backgroundUpdateStatus.RunWorkerAsync(text);
                    }
                }
            }
        }

        void newStatusTextBox_KeyUp(object sender, KeyEventArgs e)
        {
        	RefreshRemainingCharacters();
        }

		private void RefreshRemainingCharacters()
		{
            lblCharsLeft.Text = "-" + (140 - newStatusTextBox.Text.Length) + "-";
		}

		private void SetContent(string output, string container, Timeline timeLine)
		{
			//webBrowser1.Document.InvokeScript("setContents", new object[] {output, container});

			
            if (_hasUpdates)
			{
                if (Settings.Default.PlaySound)
                {
                    string path = Path.Combine(Environment.CurrentDirectory, "AgilityOrb.wav");
                    Sound.Play(path);
                }
				if (Settings.Default.VoiceEnabled)
				{

					//Process text output
					// - Put items in reverse order
					// - @username should translate to "Jason Alexander says to Scott Cate"
					// - pauses between tweets
					// - phrase URLs differently
					
                    //TODO: Fix this to work with the new timeline object model
                    _voice.Speak(VoiceTranslator.Translate(timeLine, _timelines), _speechFlags);
				}
                if (Settings.Default.MinimizeToTray && notifyIcon.Visible)
                {
                    ToastForm.ShowToast("There are new tweets!", null, new ToastTargetClickHandler(toastSelect));
                }
                _hasUpdates = false;
			}
		}

    	protected static void LogError(string title, string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "error.log"), true))
            {
                streamWriter.WriteLine();
                streamWriter.WriteLine(title + " at " + DateTime.Now);
                streamWriter.WriteLine(message);
                streamWriter.WriteLine();
                streamWriter.Close();
            }
        }

        protected static void LogError(string title, Exception ex)
        {
            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "error.log"), true))
            {
                streamWriter.WriteLine();
                streamWriter.WriteLine(title + " at " + DateTime.Now);
                streamWriter.WriteLine("Exception:");
                streamWriter.WriteLine(ex.Message);
                streamWriter.WriteLine();
                streamWriter.WriteLine("Stack Trace:");
                streamWriter.WriteLine(ex.StackTrace);
                streamWriter.WriteLine();
                streamWriter.Close();
            }
        }

    	#endregion

        #region TinyURL Support

        private void getTinyURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedText = newStatusTextBox.SelectedText;
            if (string.IsNullOrEmpty(selectedText))
            {
                return;
            }
            Uri newUri;
            if (!Uri.TryCreate(selectedText, UriKind.Absolute, out newUri))
            {
                return;
            }
            string tinyUrl = TinyUrl.GetTinyURL(selectedText);
            newStatusTextBox.SelectedText = tinyUrl;
        }

        #endregion

        #region Thread code

        private void backgroundGetLatest_DoWork(object sender, DoWorkEventArgs e)
		{
			bool successful = false;
			backgroundGetLatest.ReportProgress(0);

			while (!successful)
			{
                foreach (Timeline t in _timelines)
                {
                    try
                    {
                        t.StatusList = _controller.GetTimeline(t);
                        t.TimelineOutput = StatusFormatter.FormatTimelineHtml(t.StatusList);
                        if (t.ThisTimelineType == TimelineType.AllFriends && t.TimelineOutput.Length > 0) { _hasUpdates = true; }
                    }
                    catch (Exception ex)
                    {
                        LogError("Refreshing timeline", ex);
                        backgroundGetLatest.ReportProgress(1);
                        Thread.Sleep(10000);
                    }
                }
                successful = true;
			}
		}

		private void backgroundGetLatest_RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			// Reset refresh timer
			_timeRemaining = new TimeSpan(0, Settings.Default.RefreshDelay, 0);
			refreshTimer.Enabled = true;

            // Parse statuses for direct messages and update each timeline on the display.
            foreach (Timeline timeline in _timelines)
            {
                items = timeline.StatusList;
                images = new HybridDictionary();
                SetContent(timeline.TimelineOutput,timeline.HtmlContainerId, timeline);
                foreach (Status status in timeline.StatusList)
                {
                    if (status.Text.ToLower().Contains("@" + TwitterManager.Instance().CurrentUser.ScreenName.ToLower()))
                    {
                        // Check to see if this message has been processed before
                        if (!Properties.Settings.Default.ProcessedDirectMessages.Contains(status.Id))
                        {
                            //DirectMessageForm direct = new DirectMessageForm();
                            //direct.Show(this);
                            //direct.CurrentUser = TwitterManager.Instance().CurrentUser;
                            //direct.AddStatus(status);

                            //// Now, add it to the processed queue.
                            //Properties.Settings.Default.ProcessedDirectMessages.Add(status.Id);
                            //Properties.Settings.Default.Save();
                        }
                    }
                }
            }
		}

		private void backgroundGetLatest_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			switch (e.ProgressPercentage)
			{
				case 0:
					refreshTimer.Enabled = false;
					break;
				case 1:
					break;
			}
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			_timeRemaining = new TimeSpan(_timeRemaining.Ticks - TimeSpan.TicksPerSecond);
			if (_timeRemaining.Ticks > TimeSpan.TicksPerSecond)
			{
                string timeBreakdown = (_timeRemaining.Minutes > 0 ? _timeRemaining.Minutes + "m" : "");
                timeBreakdown += _timeRemaining.Seconds + "s";

				refreshTimer.Enabled = true;
			}
			else
			{
                if (!backgroundGetLatest.IsBusy)
                {
                    backgroundGetLatest.RunWorkerAsync();
                }
			}
		}

		private void backgroundUpdateStatus_DoWork(object sender, DoWorkEventArgs e)
		{
			bool successful = false;
			string text = e.Argument as string;
			if (text == null)
			{
			    return;
			}
			backgroundUpdateStatus.ReportProgress(0);

			while (!successful)
			{
				try
				{
					_controller.UpdateUserStatus(text);
					successful = true;
				}
				catch (Exception ex)
				{
					LogError("Updating status", ex);
					backgroundUpdateStatus.ReportProgress(1);
					Thread.Sleep(10000);
				}
			}
		}

		private void backgroundUpdateStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			_timeRemaining = new TimeSpan(0, 0, 5);
		}

		private void backgroundUpdateStatus_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			switch(e.ProgressPercentage)
			{
				case 0:
					break;
				case 1:
					break;
			}
		}

		#endregion

        #region Systray Support

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
  //          if (Settings.Default.MinimizeToTray && this.WindowState == FormWindowState.Minimized)
  //          {
  //              notifyIcon.Visible = true;
  //              this.Visible = false;
  //          }
        }

        #endregion

        #region Toast Support

        private void toastSelect(object sender, ToastSelectEventArgs args)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }
        #endregion

        private void webBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!newStatusTextBox.Focused) newStatusTextBox.Focus();
        }

        Size mouseDistance;
        bool isDrag;

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDistance = new Size(MousePosition.X - Location.X, MousePosition.Y - Location.Y);

            isDrag = e.Button == MouseButtons.Left;
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDrag = !(e.Button == MouseButtons.Left);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                this.Location = new Point(MousePosition.X -
                   mouseDistance.Width, MousePosition.Y -
                   mouseDistance.Height);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            GraphicsPath path = Helper.GetRoundRectPath(rect, 8);
            this.Region = new Region(path);

            rect = new Rectangle(picUser.Location, picUser.Size);
            path = Helper.GetRoundRectPath(rect, 8);
            picUser.Region = new Region(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void switchTimer_Tick(object sender, EventArgs e)
        {
            images[items[current].User.ScreenName] = picUser.Image;

            current--;
            if (current < 0) current = items.Count - 1;

            UpdateStatus(current);
        }

        private void rtfStatus_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void MainForm_MouseEnter(object sender, EventArgs e)
        {
            switchTimer.Enabled = false;
        }

        private void MainForm_MouseLeave(object sender, EventArgs e)
        {
            switchTimer.Enabled = true;
        }

        private void optionsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (OptionsForm form = new OptionsForm())
            {
                form.ShowDialog();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rtfStatus_MouseEnter(object sender, EventArgs e)
        {
            //rtfStatus.BackColor = Color.FromArgb(0x5E, 0x64, 0x6B);
        }

        private void rtfStatus_MouseLeave(object sender, EventArgs e)
        {
            //rtfStatus.BackColor = Color.FromArgb(47, 50, 55);
        }

        void showUserPage(string user)
        {
            string link = @"http://twitter.com/" + user;
            System.Diagnostics.Process.Start(link);
        }
        private void statusText_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string link = statusText.Text.Substring(e.Link.Start, e.Link.Length);
            if (link[0] == '@') 
                showUserPage(link.Substring(1));
            else
                System.Diagnostics.Process.Start(link);
        }

        private void lblUserName_Click(object sender, EventArgs e)
        {
            showUserPage(lblUserName.Text);
        }
        
	}
    public class Helper
    {
        private Helper()
        {
        }

        public static GraphicsPath GetRoundRectPath(RectangleF rect, float radius)
        {
            return GetRoundRectPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }
        public static GraphicsPath GetRoundRectPath(float X, float Y, float width, float height, float radius)
        {

            GraphicsPath gp = new GraphicsPath();

            gp.AddLine(X + radius, Y, X + width - (radius * 2), Y);

            gp.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);

            gp.AddLine(X + width, Y + radius, X + width, Y + height - (radius * 2));

            gp.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);

            gp.AddLine(X + width - (radius * 2), Y + height, X + radius, Y + height);

            gp.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);

            gp.AddLine(X, Y + height - (radius * 2), X, Y + radius);

            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);

            gp.CloseFigure();

            return gp;
        }
    }
}
