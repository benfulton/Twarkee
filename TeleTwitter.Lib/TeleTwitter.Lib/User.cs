using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Web;

namespace TeleTwitter.Lib
{

    public class User
    {
        #region Private Members

        private string _checksum;
        private string _description;
        private string _id;
        private string _location;
        private string _name;
        private string _profileImageUrl;
        private string _screenName;
        private Status _status;
        private string _url;

        #endregion

        #region Properties

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
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

        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string ProfileImageUrl
        {
            get
            {
                return _profileImageUrl;
            }
            set
            {
                _profileImageUrl = value;
            }
        }

        public string ScreenName
        {
            get
            {
                return _screenName;
            }
            set
            {
                _screenName = value;
            }
        }

        public Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        #endregion

        #region Constructor(s)

        public User()
        {
            _id = string.Empty;
            _name = string.Empty;
            _screenName = string.Empty;
            _location = string.Empty;
            _description = string.Empty;
            _profileImageUrl = string.Empty;
            _url = string.Empty;
            _checksum = string.Empty;
            //_status = new Status();
        }

        public User(string id, string name, string screenName, string location, string description, string profileImageUrl, string url, Status status) : this()
        {
            _id = id;
            _name = name;
            _screenName = screenName;
            _location = location;
            _description = description;
            _profileImageUrl = profileImageUrl;
            _url = url;
            _status = status;
        }

        #endregion

        #region Load Methods

        #region JSON
        public static User FromJson(string text)
        {
            User rtn = new User();

            rtn = Json.Helper.Deserialize<User>(text);

            return rtn;
        }

        public string ToJson()
        {
            return Json.Helper.Serialize<User>(this);
        }
        #endregion

        #region XML
        public static User Load(XmlElement element)
        {
            User user = new User();
            user.Id = element["id"].InnerText;
            user.Name = Util.RemoveEncoding(element["name"].InnerText);
            user.ScreenName = element["screen_name"].InnerText;
            user.Location = Util.RemoveEncoding(element["location"].InnerText);
            user.Description = Util.RemoveEncoding(element["description"].InnerText);
            user.ProfileImageUrl = element["profile_image_url"].InnerText;
            user.Url = element["url"].InnerText;
            return user;
        }
        #endregion

        #endregion
    }
}

