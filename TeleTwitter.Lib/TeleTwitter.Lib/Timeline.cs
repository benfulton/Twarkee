using System;
using System.Collections.Generic;
using System.Text;

namespace TeleTwitter.Lib
{
    public class Timeline
    {
        private List<Status> _statusList;
        public List<Status> StatusList
        {
            get { return _statusList; }
            set { _statusList = value; }
        }

        private string _timelineName="";
        public string TimelineName
        {
            get { return _timelineName; }
            set { _timelineName = TimelineName; }
        }

        private TimelineType _thisTimelineType;
        public TimelineType ThisTimelineType
        {
            get { return _thisTimelineType; }
            set { _thisTimelineType = value; }
        }

        private string _friendUsername = "";
        public string FriendUsername
        {
            get { return _friendUsername; }
            set { _friendUsername = value; }
        }

        private string _timelineOutput = "";
        public string TimelineOutput
        {
            get { return _timelineOutput; }
            set { _timelineOutput = value; }
        }

        private string _htmlContainerId = "";
        public string HtmlContainerId
        {
            get { return _htmlContainerId; }
            set { _htmlContainerId = value; }
        }

    }
}
