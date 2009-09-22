using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using TeleTwitter.Lib;
using TeleTwitter;

namespace Twarkee
{
    [ComVisible(true)]
    public class BrowserController
    {
        #region UserNameChanged Event + EventArgs

        public delegate void UserNameChangedHandler(object sender, UserNameChangeEventArguments e);
        public event UserNameChangedHandler UserNameChanged;
        public class UserNameChangeEventArguments : EventArgs
        {
            private readonly string commandText;
            /// <summary>
            /// Create User Name Changed Event Args
            /// </summary>
            /// <param name="commandText">command text will preceed the user name <example>d == Direct Message</example> and creates "d @username"</param>
            public UserNameChangeEventArguments(string commandText)
            {
                this.commandText = commandText;
            }

            public string CommandText
            {
                get
                {
                    if (commandText == string.Empty)
                    {
                        //no command, just direct reply to user
                        return "@";
                    }
                    else
                    {
                        //send command, just direct reply to user
                        return commandText.ToLower() + " ";
                    }
                }
            }

            public UserNameChangeEventArguments() {}
        }

        #endregion

        public BrowserController()
        {

            //Set client details. Does not seem to be working yet
            TwitterManager.SetClientAppDetails("TeleTwitter", "http://teletwitter.com", typeof(MainForm).Assembly.GetName().Version);
        }

        #region Get* Methods

        public List<Status> GetTimeline(Timeline timeline)
        {
            StringBuilder output = new StringBuilder();
            return TwitterManager.Instance().GetTimeline(timeline);
        }

        public void SetReply(string username)
        {
            SetReplyCommand(username, string.Empty);
        }

        public void SetReplyCommand(string username, string command)
        {
            if (UserNameChanged != null)
            {
                UserNameChanged(username, new UserNameChangeEventArguments(command));
            }
        }

        public string GetRefreshDelay()
        {
            return (Properties.Settings.Default.RefreshDelay * 60000).ToString();
        }

        #endregion

        #region Update Methods

        public string UpdateUserStatus(string newStatus)
        {
            try
            {
                Status status = TwitterManager.Instance().UpdateStatus(newStatus);
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
            return string.Empty;
        }

        #endregion

        #region UI Methods

        public void OpenLinkInDefaultBrowser(string link)
        {
            Process.Start(link);
        }

        public void OpenOptionsDialog()
        {
            using (OptionsForm form = new OptionsForm())
            {
                form.ShowDialog();
            }
        }

        public string GetPublicVisibility()
        {
                return "none";
        }

        #endregion
    }
}
