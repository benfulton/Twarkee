using System;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Collections.Generic;

namespace TeleTwitter.Lib 
{
    public class TwitterManager
    {
        #region Settings

        static TwitterManager()
        {
            _version = typeof(TwitterManager).Assembly.GetName().Version;
        }

        public static void SetClientAppDetails(string name, string link, Version ver)
        {
            _clientName = name;
            _clientLink = link;
            _version = ver;
        }

        #endregion

        #region Private Members

        private static string _password = string.Empty;
        private static string _username = string.Empty;
        private User _currentUser = null;

        // NOTE: all API URLs should use param 0 for the file type (i.e. - xml or json)
        // The SendRequest method will format the URL strings with param 0 as the selected API file extension
        // and param 1 as the "command"
        private const string ANOTHER_FRIENDS_TIMELINE_URL = "http://twitter.com/statuses/friends_timeline/{1}.{0}";
        private const string FRIENDS_TIMELINE_URL = "http://twitter.com/statuses/friends_timeline.{0}";
        private const string PUBLIC_TIMELINE_URL = "http://twitter.com/statuses/public_timeline.{0}";
        private const string UPDATE_STATUS_URL = "http://twitter.com/statuses/update.{0}?status={1}";
        private const string USER_URL = "http://twitter.com/users/show/{1}.{0}";

        private static string _clientName = "TeleTwitter";
        private static string _clientLink = "http://www.teletwitter.com";
        private static Version _version = null;
        private static TwitterManager _twitterManager;

        #endregion

        #region Constructor(s)

        private TwitterManager(string username, string password)
        {
            _username = username;
            _password = password;
            LoadCurrentUser();
        }

        private TwitterManager()
        {
        }

        #endregion

        #region Methods

        public static TwitterManager Instance()
        {
            if (_twitterManager == null)
            {
                _twitterManager = new TwitterManager();
            }
            if (_twitterManager.Username != string.Empty && _twitterManager.Password != string.Empty)
            {
                _twitterManager.LoadCurrentUser();
            }
            return _twitterManager;
        }

        #region Request methods
        /// <summary>
        /// Sends request to specified URL and returns request as string
        /// </summary>
		/// <param name="methodType">the method type of the request</param>
		/// <param name="targetUrl">the url to request</param>
        /// <returns>the response as a string</returns>
		public string SendRequest(MethodType methodType, string targetUrl)
        {
			return SendRequest(methodType, targetUrl, string.Empty, DateTime.MinValue);
        }

		/// <summary>
		/// Sends request to specified URL and returns request as string
		/// </summary>
		/// <param name="methodType">the method type of the request</param>
		/// <param name="targetUrl">the url to request</param>
		/// <param name="ifModifiedSince">the If-Modified-Since HTTP header.</param>
		/// <returns>the response as a string</returns>
		public string SendRequest(MethodType methodType, string targetUrl, DateTime ifModifiedSince)
		{
			return SendRequest(methodType, targetUrl, string.Empty, ifModifiedSince);
		}

		/// <summary>
        /// Sends request to specified URL and returns request as string
        /// </summary>
		/// <param name="methodType">the method type of the request</param>
		/// <param name="targetUrl">the url to request</param>
        /// <param name="command">the command to include in the request</param>
        /// <returns>the response as a string</returns>
		public string SendRequest(MethodType methodType, string targetUrl, string command)
        {
			return SendRequest(methodType, targetUrl, command, DateTime.MinValue);
        }
        /// <summary>
        /// Sends request to specified URL and returns request as string
        /// </summary>
		/// <param name="methodType">the method type of the request</param>
		/// <param name="targetUrl">the url to request</param>
        /// <param name="command">the command to include in the request</param>
		/// <param name="ifModifiedSince">the If-Modified-Since HTTP header.</param>
		/// <returns>the response as a string</returns>
		public string SendRequest(MethodType methodType, string targetUrl, string command, DateTime ifModifiedSince)
        {
            string result = string.Empty;

            HttpWebRequest webRequest = null;
            WebResponse webResponse = null;
            try
            {
                string url = targetUrl;

                if (command != string.Empty && command != "")
                {
                    url = string.Format(url, FileExtension, HttpUtility.UrlEncode(command));
                }
                else
                {
                    url = string.Format(url, FileExtension);
                }

                webRequest                                  = (HttpWebRequest)WebRequest.Create(url);
				if (methodType == MethodType.Post)
					webRequest.Method                       = "POST";
				else
					webRequest.Method                       = "GET";
                webRequest.ServicePoint.Expect100Continue   = false;
                webRequest.KeepAlive                        = false;
                webRequest.Credentials                      = GetCredentials();
                webRequest.Headers.Add("X-Twitter-Client", _clientName);
                webRequest.Headers.Add("X-Twitter-Client-URL", _clientLink);
                webRequest.Headers.Add("X-Twitter-Client-Version", _version.ToString());
				if (ifModifiedSince > DateTime.MinValue)
                    webRequest.IfModifiedSince = ifModifiedSince;

                // API specific settings
                switch (TwitterApi)
                {
                    case ApiType.JSON:
                        webRequest.ContentType = "application/json";
                        webRequest.Accept = "application/json";
                        break;
                    case ApiType.XML:
                        webRequest.ContentType = "application/xml";
                        webRequest.Accept = "application/xml";
                        break;
                }
                
                webResponse = webRequest.GetResponse();

                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            catch (WebException webException)
            {
                throw webException;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                try
                {
                    if (webRequest != null)
                    {
                        webRequest.GetRequestStream().Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.GetResponseStream().Close();
                    }
                }
                catch
                {
                    
                }
            }
            return result;
        }
        #endregion

        public Status UpdateStatus(string newStatus)
        {
            Status status = null;
            switch (TwitterApi)
            {
                case ApiType.XML:
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(SendRequest(MethodType.Post, UPDATE_STATUS_URL, newStatus));
                        status = Status.Load((XmlElement)document.SelectNodes("/status")[0]);
                        break;
                    }
                case ApiType.JSON:
                    {
						status = Status.FromJson(SendRequest(MethodType.Post, UPDATE_STATUS_URL, newStatus));
                        break;
                    }
            }
            return status;
        }

        private void LoadCurrentUser()
        {
            if (_currentUser != null)
            {
                return;
            }
            _currentUser = GetUser(_username);
        }

        public User GetUser(string userName)
        {
            User user = null;

            switch (TwitterApi)
            {
                case ApiType.XML:
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(SendRequest(MethodType.Get, USER_URL, userName));
                        user = User.Load((XmlElement)document.SelectNodes("/user")[0]);
                        break;
                    }
                case ApiType.JSON:
                    {
                        user = User.FromJson(SendRequest(MethodType.Get, USER_URL, userName));
                        break;
                    }
            }

            return user;
        }

        private NetworkCredential GetCredentials()
        {
            return new NetworkCredential(_username, _password);
        }

		public List<Status> GetTimeline(Timeline timeline)
		{
			return GetTimeline(timeline, DateTime.MinValue);
		}

        public List<Status> GetTimeline(Timeline timeline, DateTime since)
        {
            List<Status> list = null;

            switch (TwitterApi)
            {
                case ApiType.XML:
                    {
                        XmlDocument document = new XmlDocument();
						document.LoadXml(SendRequest(MethodType.Get, GetTimelineUrl(timeline), since));
                        list = Status.LoadStatusList(document);
                        break;
                    }
                case ApiType.JSON:
                    {
						list = Status.FromJsonList(SendRequest(MethodType.Get, GetTimelineUrl(timeline), since));
                        break;
                    }
            }

            return list;
        }

        private string GetTimelineUrl(Timeline timeline)
        {
            // handle invalid friend username case for single friend timeline??
            // NOTE: this logic was moved from the Timeline class to centralize the generation of URLs
            // probably needs revisited
            switch (timeline.ThisTimelineType)
            {
                case TimelineType.SingleFriend:
                    if( string.IsNullOrEmpty(timeline.FriendUsername)){ timeline.ThisTimelineType = TimelineType.AllFriends;}
                    break;
                default:
                    break;
            }
            return GetTimelineUrl(timeline.ThisTimelineType);
        }
        private string GetTimelineUrl(TimelineType timelineType)
        {
            switch (timelineType)
            {
                case TimelineType.Public:
                    return PUBLIC_TIMELINE_URL;
                case TimelineType.AllFriends:
                    return FRIENDS_TIMELINE_URL;
                case TimelineType.SingleFriend:
                    return ANOTHER_FRIENDS_TIMELINE_URL;
                default:
                    return string.Empty;
            }
        }
        
        #endregion

        #region Properties

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    LoadCurrentUser();
                }
                return _currentUser; 
            }
            set { _currentUser = value; }
        }

        private ApiType _twitterApi = ApiType.XML;

        /// <summary>
        /// The Twitter API to use.  Default is XML.
        /// </summary>
        public ApiType TwitterApi
        {
            get { return _twitterApi; }
            set { _twitterApi = value; }
        }

        private string FileExtension
        {
            get
            {
                string rtn = string.Empty;
                switch (TwitterApi)
                {
                    case ApiType.XML:
                        rtn = "xml";
                        break;
                    case ApiType.JSON:
                    default:
                        rtn = "json";
                        break;
                }
                return rtn;
            }
        }
	
        #endregion
    }
}

