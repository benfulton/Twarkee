using System;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace TeleTwitter.Lib
{
    public class Status
    {
        #region Private Members

        private DateTime _createdAt;
        private string _id;
        private string _text;
        private User _user;

        #endregion

        #region Constructor(s)

        public Status() : this(string.Empty, DateTime.MinValue, string.Empty)
        {
            _user = new User();
        }

        public Status(string id, DateTime createdAt, string text)
        {
            _id = string.Empty;
            _createdAt = DateTime.MinValue;
            _text = string.Empty;
            _id = id;
            _createdAt = createdAt;
            _text = text;
        }

        #endregion

        #region Load Methods

        #region JSON
        public static Status FromJson(string text)
        {
            Status rtn = new Status();

            rtn = Json.Helper.Deserialize<Status>(text);

            return rtn;
        }

        public static List<Status> FromJsonList(string list)
        {
            List<Status> rtn = new List<Status>();

            rtn = Json.Helper.Deserialize<List<Status>>(list);

            return rtn;
        }

        public string ToJson()
        {
            return Json.Helper.Serialize<Status>(this);
        }
        #endregion

        #region XML
        public static Status Load(XmlElement element)
        {
            Status status = new Status();

            status.CreatedAt = DateTime.ParseExact(element["created_at"].InnerText, "ddd MMM dd HH:mm:ss zzzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            status.Id = element["id"].InnerText;
            status.Text = Util.RemoveEncoding(element["text"].InnerText);

            XmlNodeList userNodes = element.SelectNodes("user");

            if (userNodes.Count >= 1)
            {
                status.User = User.Load((XmlElement)userNodes[0]);
            }

            return status;
        }

        public static List<Status> LoadStatusList(XmlDocument xml)
        {
            List<Status> statuses = new List<Status>();

            if (xml.ChildNodes.Count >= 1)
            {
                XmlNodeList statusNodes = xml.ChildNodes[1].SelectNodes("/statuses/status");

                foreach (XmlElement element in statusNodes)
                {
                    statuses.Add(Status.Load(element));
                }

            }

            return statuses;
        }
        #endregion

        #endregion

        #region Properties

        public User User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
            }
        }

        public DateTime CreatedAt
        {
            get
            {
                return _createdAt;
            }
            set
            {
                _createdAt = value;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string RelativeCreatedAt
        {
            get
            {
                TimeSpan span = DateTime.Now.Subtract(CreatedAt);
                int days = Convert.ToInt32(span.TotalDays);
                int hours = Convert.ToInt32(span.TotalHours);
                int minutes = Convert.ToInt32(span.TotalMinutes);
                int seconds = Convert.ToInt32(span.TotalSeconds);

                if (days > 0)
                {
                    return string.Format("About {0} day{1} ago", days, days > 1 ? "s" : "");
                }
                if (hours > 0)
                {
                    return string.Format("About {0} hour{1} ago", hours, hours > 1 ? "s" : "");
                }
                if (minutes > 0)
                {
                    return string.Format("About {0} minute{1} ago", minutes, minutes > 1 ? "s" : "");
                }
                if (seconds < 0)
                {
                    seconds = 0;
                }
                return string.Format("{0} second{1} ago", seconds, (seconds > 1 || seconds == 0) ? "s" : "");
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        #endregion



    }
}

